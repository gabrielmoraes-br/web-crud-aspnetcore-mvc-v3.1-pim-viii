using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebCRUD_PIM_VIII.Models
{
    public class Telefone
    {
        public int Id { get; set; }

        [Display(Name = "Número")]
        public int Numero { get; set; }
        public int DDD { get; set; }

        [Required]
        public TipoTelefone TipoTelefone { get; set; }

        public override string ToString()
        {
            return $"{Id}, {Numero}, {DDD}, {TipoTelefone}";
        }
    }
}
