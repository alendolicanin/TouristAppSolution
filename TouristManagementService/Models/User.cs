using Microsoft.AspNetCore.Identity;

namespace TouristManagementService.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public Role Role { get; set; }
    }
}
