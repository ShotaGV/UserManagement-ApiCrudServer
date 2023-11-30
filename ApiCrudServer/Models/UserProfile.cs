using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiCrudServer.Models
{
    public class UserProfile
    {
        public int Id { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [StringLength(11)]
        public int PhoneNumber { get; set; }       
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

    }
}
