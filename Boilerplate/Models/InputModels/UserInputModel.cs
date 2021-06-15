using System.ComponentModel.DataAnnotations;

namespace Application.Models.InputModels
{
    public record UserInputModel
    {
        [Required]
        public string Name { get; init; }
        
        [Required]
        public string LastName { get; init; }
        
        [Required]
        [EmailAddress]
        public string Email { get; init; }

        [Required]
        [MinLength(3)]
        public string Password { get; init; }
    }
}
