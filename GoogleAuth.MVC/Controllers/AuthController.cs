using GoogleAuth.MVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GoogleAuth.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Register()
        {
            var externalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return View(externalLogins);
        }

        [HttpPost]
        public IActionResult GoogleLogin(string provider)
        {
            var redirectUrl = Url.Action("GoogleLoginCallBack","Auth");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider,redirectUrl);
            return Challenge(properties,provider);
        }

        public async Task<IActionResult> GoogleLoginCallBack()
        {
            var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();

            if(externalLoginInfo == null)
            {
                return RedirectToAction("Login");
            }

            var email=externalLoginInfo.Principal.FindFirst(ClaimTypes.Email).Value;

            if(email == null) {
                return RedirectToAction("Login");
            }

            var user=await _signInManager.UserManager.FindByEmailAsync(email);

            if(user == null)
            {
                //Google Register
                return RedirectToAction("GoogleRegister", new { email });
            }

            return View();
        }

        public IActionResult GoogleRegister(string email)
        {
            RegisterVM registerVM=new RegisterVM();
            registerVM.Email = email;

            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> GoogleRegister(RegisterVM registerVM)
        {
            var user = new IdentityUser
            {
                Email = registerVM.Email,
            };

            if (user.UserName == null)
            {
                user.UserName = user.Email;
            }
            else
            {
                user.UserName = registerVM.Username;
            }

            var result=await _signInManager.UserManager.CreateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index","Home");
            }

            return View();
        }
    }
}
