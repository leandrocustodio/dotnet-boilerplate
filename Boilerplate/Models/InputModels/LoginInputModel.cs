using System.ComponentModel.DataAnnotations;

namespace Application.Models.InputModels
{
    public class LoginInputModel
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        
        [Required]
        [MinLength(3)]
        public string Password { get; set; }
    }
}
