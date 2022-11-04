using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebCRUD_PIM_VIII.Models
{
    public class Endereco
    {
        public int Id { get; set; }

        public string Logradouro { get; set; }

        [Display(Name = "Número")]
        public int Numero { get; set; }
        public int Cep { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public string Estado { get; set; }

        public override string ToString()
        {
            return $"{Id}, {Logradouro}, {Numero}";
        }
    }
}