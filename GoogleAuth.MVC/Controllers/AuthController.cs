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

            }

            return View();
        }
    }
}
