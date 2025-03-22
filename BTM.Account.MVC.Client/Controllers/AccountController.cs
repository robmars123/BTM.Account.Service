using BTM.Account.MVC.Client.Models;
using BTM.Account.MVC.Client.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

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
                if (model.Password != model.ConfirmPassword)
                {
                    // This check is redundant due to the [Compare] attribute
                    ModelState.AddModelError(string.Empty, "Passwords do not match.");
                    return View(model);
                }

                UserRequestModel request = new UserRequestModel
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = model.Password
                };

                var httpClient = _httpClientFactory.CreateClient("AccountAPI");

                // Serialize the model to JSON to send in the request body
                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json"  // Specify that the content type is JSON
                );

                var httpRequest = new HttpRequestMessage(
                    HttpMethod.Post,
                    "/api/users/") // Ensure this matches the correct API endpoint for user registration
                {
                    Content = jsonContent  // Add the serialized model as content to the request
                };

                var response = await httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);

                if (!response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();

                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseJson);

                    // You can access the specific error details
                    var errorCode = errorResponse?.code;
                    var errorMessage = errorResponse?.name ?? "Unknown error";

                    // Optionally, log the error or display it in a user-friendly way
                    var result = Result.FailureResult(errorMessage);

                    // Add the error message to ModelState for displaying in the registration view
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
