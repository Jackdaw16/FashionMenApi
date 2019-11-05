namespace fashionMenApi.Models
{
    public class Pedido
    {
        public int idPed { get; set; }
        public bool entregado { get; set; }
        
        public int cantidad_producto { get; set; }
        
        public string talla_seleccionada { get; set; }
        
        public double precio_total { get; set; }
        public int id_usu { get; set; }
        public int id_producto { get; set; }
        
        public virtual Usuario usuario { get; set; }
        public virtual Producto producto { get; set; }
    }
}