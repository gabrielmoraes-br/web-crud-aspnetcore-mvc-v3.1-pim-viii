using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebCRUD_PIM_VIII.Models
{
    public class Pessoa
    {
        public int Id { get; set; }

        [Display(Name = "Nome completo")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "O nome completo precisa ter entre 3 e 60 caracteres.")]
        [Required]
        public string Nome { get; set; }

        [RegularExpression(@"\d{11}", ErrorMessage = "Precisa ter 11 dígitos.")]
        [Required]
        public long CPF { get; set; }

        [Required]
        public Endereco Endereco { get; set; }

        [Required]
        public Telefone[] Telefones { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Id}, ");
            sb.Append($"{Nome}, ");
            sb.Append($"{CPF}");
            sb.Append("\n");
            sb.Append($"{Endereco}");
            sb.Append("\n");
            foreach (Telefone telefone in Telefones)
            {
                sb.Append(telefone);
                sb.Append("\n");
            }
            return sb.ToString();
        }
    }
}