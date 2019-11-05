using System.Collections.Generic;

namespace fashionMenApi.Models
{
    public class Usuario
    {
        public int id { get; set; }
        public string dni { get; set; }
        public string nombre_usuario { get; set; }
        public string passwd { get; set; }
        public string nombre_completo { get; set; }
        public string correo_electronico { get; set; }
        public string fecha_nacimiento { get; set; }
        public string direccion { get; set; }
        
        public virtual List<Pedido> pedidos { get; set; }
    }
}