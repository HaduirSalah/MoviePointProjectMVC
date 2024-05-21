using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoviePoint.Models;
using MoviePoint.ViewModel;

namespace MoviePoint.Controllers
{
    [Authorize(Roles="Admin")]
    public class RoleController : Controller
    {
		private readonly UserManager<IdentityUser> userManager;
		MoviePointContext context = new MoviePointContext();

		public RoleController(UserManager<IdentityUser> UserManager)
        {
            userManager = UserManager;
		}

        public IActionResult AssigRole()
        {
            RoleViewModel roleView=new RoleViewModel();
			roleView.Users = context.Users.ToList();
			roleView.Roles = context.Roles.ToList();

			return View(roleView);
        }

        [HttpPost]
		public async Task<IActionResult> AssigRole(RoleViewModel RoleVM)
		{
			if (ModelState.IsValid)
            {
                IdentityUserRole<string> roleModel = new IdentityUserRole<string>();

				roleModel.UserId = RoleVM.UserID;
				roleModel.RoleId = RoleVM.RoleID;
                IdentityUser SelectedUser = context.Users.FirstOrDefault(u => u.Id == RoleVM.UserID);
                string RoleName = context.Roles.FirstOrDefault(r => r.Id == roleModel.RoleId).Name;
				IdentityResult result = await userManager.AddToRoleAsync(SelectedUser, RoleName);

                if (result.Succeeded)
                {
                    return View("Home");
                }
                else
                {
                    ModelState.AddModelError("", result.Errors.FirstOrDefault().Description);
                }

           }
            return View(RoleVM);
        }
	}
}
