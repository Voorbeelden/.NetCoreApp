using Microsoft.AspNetCore.Identity;

namespace Calenderapp.MVC.Models.ViewModels
{
    public class GebruikerRechtenViewModel
    {
        public required IdentityUser User { get; set; }
        public required List<IdentityRole> Roles { get; set; }
        public required List<string> SelectedRoleIds { get; set; }
    }
}
