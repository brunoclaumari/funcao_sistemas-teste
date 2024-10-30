using System.ComponentModel.DataAnnotations;

namespace WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Classe de Modelo de Beneficiario
    /// </summary>
    public class BeneficiarioModel
    {
        public long Id { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        [Required]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "Digite um CPF válido")]
        public string Cpf { get; set; }


        /// <summary>
        /// Nome do beneficiário
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// Cidade
        /// </summary>
        [Required]
        public long IdCliente { get; set; }
    }
}