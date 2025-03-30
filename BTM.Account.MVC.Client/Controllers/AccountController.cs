using Azure.Core;
using BTM.Account.Application.Factories.HttpRequest;
using BTM.Account.Domain.Users;
using BTM.Account.MVC.UI.Models;
using BTM.Account.MVC.UI.Models.Commands;
using BTM.Account.MVC.UI.Models.Requests;
using BTM.Account.MVC.UI.Models.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;

namespace BTM.Account.MVC.Client.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRequestFactory _httpRequest;
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IRequestFactory httpRequestFactory, IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _httpRequest = httpRequestFactory;
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            //get user's identity
            string? id = User.FindFirstValue("sub");

            if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(accessToken))
                return View();

            var response = await _httpRequest.GetRequestAsync($"api/users/{id}", null, accessToken ?? string.Empty);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeObject<Result>(await response.Content.ReadAsStringAsync());
                if (errorResponse != null)
                {
                    foreach (var error in errorResponse.ErrorMessages)
                        ModelState.AddModelError(string.Empty, error);
                }

                return View(new UserDTO());
            }

            var user = JsonConvert.DeserializeObject<UserDTO>(await response.Content.ReadAsStringAsync());

            return View(user);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            var request = new UserRequestCommand(model.Email,model.Username,model.Password);

            var response = await _httpRequest.SendPostRequestAsync("api/users", request, string.Empty);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonConvert.DeserializeObject<Result>(await response.Content.ReadAsStringAsync());

                if (errorResponse != null)
                {
                    foreach (var error in errorResponse.ErrorMessages)
                        ModelState.AddModelError(string.Empty, error);
                }

                return View(RegisterRequest.Reset()); 
            }

            return RedirectToAction("Login", "Account");
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
