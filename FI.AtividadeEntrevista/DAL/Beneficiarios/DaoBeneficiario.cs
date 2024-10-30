using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FI.AtividadeEntrevista.DAL
{
    /// <summary>
    /// Classe de acesso a dados de Cliente
    /// </summary>
    internal class DaoBeneficiario : AcessoDados
    {
        /*
          “ID”, “CPF”, “NOME”, “IDCLIENTE”
         */


        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="beneficiario">Objeto de cliente</param>
        internal long Incluir(DML.Beneficiario beneficiario)
        {
            /*
             	@ID				BIGINT = NULL,
                @NOME          VARCHAR (50) ,
                @CPF           VARCHAR (14),
	            @IDCLIENTE		BIGINT
             */

            List<System.Data.SqlClient.SqlParameter> parametros = RetornaParametros(beneficiario);

            DataSet ds = base.Consultar("FI_SP_InsereOuAlteraBeneficiario", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        ///// <summary>
        ///// Inclui um novo cliente
        ///// </summary>
        ///// <param name="cliente">Objeto de cliente</param>
        //internal void Alterar(DML.Beneficiario beneficiario)
        //{
        //    List<System.Data.SqlClient.SqlParameter> parametros = RetornaParametros(beneficiario);

        //    base.Executar("FI_SP_InsereOuAlteraBeneficiario", parametros);
        //}

        internal List<System.Data.SqlClient.SqlParameter> RetornaParametros(Beneficiario beneficiario)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>
            {
                new System.Data.SqlClient.SqlParameter("ID", beneficiario.Id),
                new System.Data.SqlClient.SqlParameter("Nome", beneficiario.Nome),
                new System.Data.SqlClient.SqlParameter("Cpf", beneficiario.Cpf),
                new System.Data.SqlClient.SqlParameter("IDCLIENTE", beneficiario.IdCliente)
            };

            return parametros;
        }

        ///// <summary>
        ///// Inclui um novo cliente
        ///// </summary>
        ///// <param name="cliente">Objeto de cliente</param>
        //internal DML.Cliente Consultar(long Id)
        //{
        //    List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

        //    parametros.Add(new System.Data.SqlClient.SqlParameter("Id", Id));

        //    DataSet ds = base.Consultar("FI_SP_ConsCliente", parametros);
        //    List<DML.Cliente> cli = Converter(ds);

        //    return cli.FirstOrDefault();
        //}

        internal bool VerificarExistenciaCpfBeneficiario(string CPF, long id)
        {
            bool retornoCpfExiste = false;
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", CPF));

            DataSet ds = base.Consultar("FI_SP_VerificaCpfBeneficiario", parametros);

            if (id <= 0)//id <= 0 é registro novo
                return ds.Tables[0].Rows.Count > 0;
            else if (ds.Tables[0].Rows.Count > 0)
            {
                long idConsultado;
                var resultado = ds.Tables[0].Rows[0]["ID"];
                if (resultado != null && long.TryParse(resultado.ToString(), out idConsultado))
                {
                    retornoCpfExiste = id != idConsultado;
                }
            }
            return retornoCpfExiste;
        }



        

        /// <summary>
        /// Lista todos os clientes
        /// </summary>
        internal List<Beneficiario> Listar()
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", 0));

            DataSet ds = base.Consultar("FI_SP_ConsBeneficiario", parametros);
            List<Beneficiario> beneficiarios = Converter(ds);

            return beneficiarios;
        }


        /// <summary>
        /// Excluir Cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        internal void Excluir(long Id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", Id));

            base.Executar("FI_SP_DelBeneficiario", parametros);
        }

        private List<Beneficiario> Converter(DataSet ds)
        {
            /*
             “ID”, “CPF”, “NOME”, “IDCLIENTE”
            */


            List<Beneficiario> lista = new List<Beneficiario>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Beneficiario beneficiario = new DML.Beneficiario();
                    beneficiario.Id = row.Field<long>("ID");
                    beneficiario.Cpf = row.Field<string>("CPF");
                    beneficiario.Nome = row.Field<string>("NOME");
                    
                    beneficiario.IdCliente = Convert.ToInt64(row.Field<string>("IDCLIENTE"));
                    lista.Add(beneficiario);
                }
            }

            return lista;
        }
    }
}
