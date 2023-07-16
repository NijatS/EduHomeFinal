using System.ComponentModel.DataAnnotations;

namespace EduHome.App.ViewModels
{
    public class UpdatedUserVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string? CurrentPassword { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string? Password { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
    }
}
