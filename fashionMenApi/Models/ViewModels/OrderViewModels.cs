namespace fashionMenApi.Models.ViewModels
{
    public class OrderResponse
    {
        public int id { get; set; }
        public bool shipped { get; set; }
        public double price { get; set; }
        
        public ProductResponse product { get; set; }
    }
    
    public class OrderDetailResponse
    {
        public int id { get; set; }
        public bool shipped { get; set; }
        
        public int product_amount { get; set; }
        
        public string selected_size { get; set; }
        
        public double price { get; set; }
        
        public ProductResponse product { get; set; }
    }
    
    public class OrderCreate
    {
        public int product_id { get; set; }
        public int product_amount { get; set; }
        public string selected_size { get; set; }
    }
}