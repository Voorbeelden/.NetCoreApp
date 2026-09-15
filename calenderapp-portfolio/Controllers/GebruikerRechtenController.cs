using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Calenderapp.MVC.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;
namespace Calenderapp.MVC.Controllers
{
    [Authorize(Roles = "admin, directeur")]
    public class GebruikerRechtenController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : Controller
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();
            var viewModel = new GebruikerRechtenViewModel
            {
                User = user,
                Roles = allRoles,
                SelectedRoleIds = userRoles.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GebruikerRechtenViewModel viewModel)
        {
            var user = await _userManager.FindByIdAsync(viewModel.User.Id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var selectedRoles = viewModel.SelectedRoleIds;

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles).ToList());
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add roles.");
                return View(viewModel);
            }

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles).ToList());
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove roles.");
                return View(viewModel);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
