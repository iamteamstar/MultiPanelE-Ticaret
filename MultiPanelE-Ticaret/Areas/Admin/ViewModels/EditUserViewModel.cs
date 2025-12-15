using System.ComponentModel.DataAnnotations;

namespace MultiPanelE_Ticaret.Areas.Admin.ViewModels
{
    public class EditUserViewModel
    {
        [Required]
        public string FullName { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Role { get; set; } = "";
    }
}
