using System.ComponentModel.DataAnnotations;

namespace Application.Models.InputModels
{
    public record LoginInputModel
    {
        [Required]
        [EmailAddress]
        public string Username { get; init; }
        
        [Required]
        [MinLength(3)]
        public string Password { get; init; }
    }
}
