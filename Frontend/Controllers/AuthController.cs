using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using P10.Frontend.Models;
using P10.Frontend.Services;

namespace P10.Frontend.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthApiClient _authApiClient;

        public AuthController(IAuthApiClient authApiClient)
        {
            _authApiClient = authApiClient;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var token = await _authApiClient.LoginAsync(model.Username, model.Password);

            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }

            // On stocke le token en session
            HttpContext.Session.SetString("JwtToken", token);

            return RedirectToAction("Index", "Patient");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JwtToken");
            return RedirectToAction("Index", "Home");
        }
    }
}
