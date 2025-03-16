using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BTM.Account.MVC.Client.ViewModels;
using BTM.Account.MVC.Client.Models;

namespace BTM.Account.MVC.Client.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ??
                throw new ArgumentNullException(nameof(httpClientFactory));
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var httpClient = _httpClientFactory.CreateClient("AccountAPI");

                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    "/api/users/");

                var response = await httpClient.SendAsync(
                    request, HttpCompletionOption.ResponseHeadersRead);

                if(!response.IsSuccessStatusCode)
                {
                    var result = Result.FailureResult("An error occurred while communicating with the API. Please try again later.");
                    // Optionally, you can add the result's message to ModelState for displaying on the registration page.
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View(model);  // Return to the registration page with the error message.
                }

                // Redirect to a success page or login page
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }


        public async Task<IActionResult> Login()
        {
            // Optionally, clear the local session (cookie) first
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to Duende Identity Server login page via OpenID Connect flow
            return Challenge(new AuthenticationProperties { RedirectUri = "/" }, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [Authorize]
        public async Task Logout()
        {

            // Clears the  local cookie
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirects to the IDP linked to scheme
            // "OpenIdConnectDefaults.AuthenticationScheme" (oidc)
            // so it can clear its own session/cookie
            await HttpContext.SignOutAsync(
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
