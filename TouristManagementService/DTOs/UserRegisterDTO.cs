using System.ComponentModel.DataAnnotations;
using TouristManagementService.Models;

namespace TouristManagementService.DTOs
{
    public class UserRegisterDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [Range(18, 65, ErrorMessage = "Age must be between 18 and 65.")]
        public int Age { get; set; }
        [Required]
        public Role Role { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }
    }
}
