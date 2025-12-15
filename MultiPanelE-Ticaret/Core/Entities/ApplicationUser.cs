using Microsoft.AspNetCore.Identity;


namespace MultiPanelE_Ticaret.Core.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string? FullName { get; set; }
    }
}
