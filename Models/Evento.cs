namespace PIABD.Models
{
    public class Evento
    {
        public int eventoID { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public DateTime fecha { get; set; }
        public int recintoID { get; set; }
        public int dependenciaID {  get; set; }
        public int categoriaID {  get; set; }



    }
}
