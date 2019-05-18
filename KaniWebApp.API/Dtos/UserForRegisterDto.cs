using System.ComponentModel.DataAnnotations;

namespace KaniWebApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        
        [Required] 
        [StringLength(24, MinimumLength = 6, ErrorMessage = "You must specify password between 6 and 24 characters")]
        public string Password { get; set; }
    }
} 