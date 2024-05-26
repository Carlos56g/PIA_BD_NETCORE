namespace PIABD.Models
{
    public class Usuario
    {
        public int usuarioID { get; set; }
        public int rolID { get; set; }
        public int dependenciaID { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string correo { get; set; }
        public string constraseña { get; set; }

    }
}
