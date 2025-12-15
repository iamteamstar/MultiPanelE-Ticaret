using System.ComponentModel.DataAnnotations;

namespace MultiPanelE_Ticaret.Areas.Admin.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string FullName { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required]
        public string Role { get; set; } = "";
    }
}
