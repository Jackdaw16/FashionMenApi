using System.Collections.Generic;

namespace fashionMenApi.Models
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string full_name { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        
        public virtual List<Order> orders { get; set; }
    }
}