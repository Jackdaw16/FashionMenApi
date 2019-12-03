namespace fashionMenApi.Models
{
    public class Order
    {
        public int id { get; set; }
        public bool shipped { get; set; }
        
        public int product_amount { get; set; }
        
        public string selected_size { get; set; }
        
        public double price { get; set; }
        public int user_id { get; set; }
        public int product_id { get; set; }
        
        public virtual User user { get; set; }
        public virtual Product product { get; set; }
    }
}