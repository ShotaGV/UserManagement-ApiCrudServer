using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiCrudServer.Models
{
    public class User   
    {
        
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public UserProfile? UserProfile { get; set; }

    }
}
