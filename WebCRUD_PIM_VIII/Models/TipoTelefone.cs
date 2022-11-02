using System.ComponentModel.DataAnnotations.Schema;

namespace WebCRUD_PIM_VIII.Models
{
    [Table("Tipo_Telefone")]
    public class TipoTelefone
    {
        public int Id { get; set; }
        public string Tipo { get; set; }

        public override string ToString()
        {
            return $"{Tipo}";
        }
    }
}

