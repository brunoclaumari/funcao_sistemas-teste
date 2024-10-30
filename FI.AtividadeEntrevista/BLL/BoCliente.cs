using FI.AtividadeEntrevista.DAL;
using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoCliente
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Cliente cliente)
        {
            long retornoId = 0L;
            using (TransactionScope transacao = new TransactionScope())
            {
                try
                {
                    DAL.DaoCliente cli = new DAL.DaoCliente();
                    retornoId = cli.Incluir(cliente);                    

                    DaoBeneficiario benDAO = new DaoBeneficiario();
                    cliente.Beneficiarios.ForEach(ben =>
                    {
                        ben.IdCliente = cliente.Id;
                        benDAO.Incluir(ben);
                    });

                    // Se tudo correr bem, confirme a transação
                    transacao.Complete();                    
                }
                catch (Exception ex)
                {
                    // O escopo será automaticamente descartado, e a transação será revertida
                    Console.WriteLine("Erro na transação: " + ex.Message);                    
                }

            }

            return retornoId;
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public bool Alterar(DML.Cliente cliente)
        {
            bool deuCerto;
            using (TransactionScope transacao = new TransactionScope())
            {
                try
                {
                    DAL.DaoCliente cli = new DAL.DaoCliente();
                    DaoBeneficiario benDAO = new DaoBeneficiario();

                    var listaIds = cliente.Beneficiarios.Where(x=>x.Id > 0).Select(x => x.Id).ToList();

                    var listaBeneficiarioAntigoParaExcluir = benDAO.Listar(cliente.Id).FindAll(x => !listaIds.Contains(x.Id));
                    listaBeneficiarioAntigoParaExcluir.ForEach(b =>
                    {                        
                        benDAO.Excluir(b.Id);
                    });

                    cli.Alterar(cliente);

                    cliente.Beneficiarios.ForEach(ben =>
                    {
                        benDAO.Incluir(ben);
                    });

                    // Se tudo correr bem, confirme a transação
                    transacao.Complete();
                    deuCerto = true;
                }
                catch (Exception ex)
                {
                    // O escopo será automaticamente descartado, e a transação será revertida
                    Console.WriteLine("Erro na transação: " + ex.Message);
                    deuCerto = false;
                }
            }

            return deuCerto;
        }



        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public DML.Cliente Consultar(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Consultar(id);
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Excluir(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Listar()
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Listar();
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Pesquisa(iniciarEm, quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// Retorna se CPF tem erros de validação ou se existe
        /// </summary>
        /// <returns></returns>
        public bool CpfComProblemas(string CPF, long id, ref List<string> erros, bool isCliente = true)
        {
            bool cpfExiste = VerificarExistencia(CPF, id, isCliente);
            bool cpfInvalido = !CpfValido(CPF);

            string nomeTipoUsuarioSistema = isCliente ? "Cliente" : "Beneficiário";

            AdicionaErrosCpf(CPF, cpfExiste, cpfInvalido, ref erros, nomeTipoUsuarioSistema);

            return cpfExiste || cpfInvalido;
        }


        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF, long id, bool isCliente)
        {
            if (isCliente)
            {
                DAL.DaoCliente cli = new DAL.DaoCliente();
                return cli.VerificarExistencia(CPF, id);
            }

            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.VerificarExistenciaCpfBeneficiario(CPF, id);
        }
        /*
         public bool VerificarExistencia(string CPF)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.VerificarExistencia(CPF);
        }
         */

        public void AdicionaErrosCpf(string numeroCpf, bool cpfExiste, bool cpfinvalido, ref List<string> erros, string nomeTipoUsuarioSistema)
        {
            //string msgCpf = string.Empty;
            string msgCpf = $"O Cpf {numeroCpf} do {nomeTipoUsuarioSistema} ";
            if (cpfExiste)
            {
                msgCpf += $"já existe!";
                erros.Add(msgCpf);
            }
            else if (cpfinvalido)
            {
                msgCpf += $"é inválido!";
                erros.Add(msgCpf);
            }
        }

        /// <summary>
        /// Código para validar CPF (obtido do site do Macoratti)
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public bool CpfValido(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;

            if (cpf.Equals("00000000000") ||
                cpf.Equals("11111111111") ||
                cpf.Equals("22222222222") ||
                cpf.Equals("33333333333") ||
                cpf.Equals("44444444444") ||
                cpf.Equals("55555555555") ||
                cpf.Equals("66666666666") ||
                cpf.Equals("77777777777") ||
                cpf.Equals("88888888888") ||
                cpf.Equals("99999999999"))
            {
                return false;
            }

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

    }
}
