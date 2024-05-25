using System.Data.SqlTypes;

namespace PIABD.Models
{
    public class Costo
    {
        public int costoID { get; set; }
        public int costo { get; set; }
        public string descripcion { get; set; }
        public int eventoID { get; set; }
    }
}
