using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoviePoint.ViewModel;
using System.Security.Claims;

namespace MoviePoint.logic.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AdminController
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
                    await userManager.AddToRoleAsync(userModel, "Admin");//insert row UserRole

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
    }
}

