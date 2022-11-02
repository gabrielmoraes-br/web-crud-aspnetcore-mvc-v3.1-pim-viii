namespace WebCRUD_PIM_VIII.Models
{
    public class Telefone
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public int DDD { get; set; }
        public TipoTelefone TipoTelefone { get; set; }

        public override string ToString()
        {
            return $"{Id}, {Numero}, {DDD}, {TipoTelefone}";
        }
    }
}
