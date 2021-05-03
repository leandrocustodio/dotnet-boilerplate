using System.ComponentModel.DataAnnotations;

namespace Application.Models.InputModels
{
    public class UserInputModel
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(3)]
        public string Password { get; set; }
    }
}
