namespace fashionMenApi.Models.ViewModels
{

    public class UserResponse
    {
        public int id { get; set; }
        public string username { get; set; }
        public string full_name { get; set; }
        public string email { get; set; }
        public string address { get; set; }
    }

    public class UserRegister
    {
        public string username { get; set; }
        public string password { get; set; }
        public string full_name { get; set; }
        public string email { get; set; }
        public string address { get; set; }
    }

    public class UserLogin
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    
    public class UserLoggedIn
    {
        public UserResponse user { get; set; }
        public string accessToken { get; set; }
    }
}