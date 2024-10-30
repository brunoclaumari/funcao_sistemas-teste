using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.DML
{
    public class Beneficiario
    {
        public long Id { get; set; }

        /// <summary>
        /// CPF
        /// </summary>        
        public string Cpf { get; set; }


        /// <summary>
        /// Nome do beneficiário
        /// </summary>        
        public string Nome { get; set; }

        /// <summary>
        /// Cidade
        /// </summary>        
        public long IdCliente { get; set; }
    }
}
