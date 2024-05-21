using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoviePoint.ViewModel;
using System.Security.Claims;

namespace MoviePoint.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController
            (UserManager<IdentityUser> _userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = _userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
      
        public IActionResult Registration()
        {
            return View();
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationViewModel newUserVM)
        {
            if (ModelState.IsValid)
            {
                IdentityUser userModel = new IdentityUser();
                userModel.UserName = newUserVM.UserName;
                userModel.PasswordHash = newUserVM.Password;
                userModel.Email = newUserVM.Email;
                userModel.PhoneNumber = newUserVM.Phone;
               

                IdentityResult result =
                    await userManager.CreateAsync(userModel, newUserVM.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(userModel, "User");
                    
                    List<Claim> addClaim = new List<Claim>();
                    addClaim.Add(new Claim("Intake", "43"));
                    await signInManager.SignInWithClaimsAsync(userModel, false, addClaim);
					return RedirectToAction("Home", "Home");
				}
				else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(newUserVM);
        }

		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel userVmReq)
		{
			if (ModelState.IsValid)
			{
				IdentityUser userModel =
					await userManager.FindByNameAsync(userVmReq.Username);
				if (userModel != null)
				{
					Microsoft.AspNetCore.Identity.SignInResult rr =
						await signInManager.PasswordSignInAsync(userModel, userVmReq.Password, userVmReq.rememberMe, false);
					if (rr.Succeeded)
						return RedirectToAction("Home","Home");
				}
				else
				{
					ModelState.AddModelError("", "User Not Found :( ");
				}
			}
			return View(userVmReq);
		}


		public async Task<IActionResult> signOut()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Login");
		}
	}
}
