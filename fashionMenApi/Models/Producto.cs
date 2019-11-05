using System.Collections.Generic;

namespace fashionMenApi.Models
{
    public class Producto
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public double precio { get; set; }
        public string img { get; set; }
        public string tipoProducto { get; set; }
        public string descripcion { get; set; }
        public string tallas { get; set; }
        public int likes { get; set; }
        
        public virtual List<Pedido> pedidos { get; set; }
    }
}