using System.Collections.Generic;

namespace fashionMenApi.Models
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public string image_url { get; set; }
        public string product_type { get; set; }
        public string description { get; set; }
        public string sizes { get; set; }
        
        public virtual ICollection<Order> orders { get; set; }
    }
}