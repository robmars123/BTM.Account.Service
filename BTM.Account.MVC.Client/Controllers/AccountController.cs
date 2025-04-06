using BTM.Account.Application.Abstractions;
using BTM.Account.Application.DTOs;
using BTM.Account.Application.Factories.HttpRequest;
using BTM.Account.Application.Results;
using BTM.Account.Application.Users.RegisterUser;
using BTM.Account.Infrastructure.Services;
using BTM.Account.MVC.UI.Controllers.Base;
using BTM.Account.MVC.UI.Models.Commands;
using BTM.Account.MVC.UI.Models.Requests;
using BTM.Account.MVC.UI.Models.Results;
using BTM.Account.Shared.Common;
using Duende.IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;

namespace BTM.Account.MVC.Client.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;

        public AccountController(IHttpContextAccessor httpContextAccessor,
                                 ITokenService tokenService,
                                 IUserService userService) : base(tokenService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var accessToken = await GetAccessTokenAsync();

            //get user's identity
            string? userId = User.FindFirstValue(JwtClaimTypes.Subject);

            if (userId == null)
                return View();

            Result<UserDTO> user = await _userService.GetUserAsync(userId, accessToken);

            if (!user.IsSuccess)
                return View();

            UserDTO userDTO = user.Data;

            return View(userDTO);
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
            var request = new RegisterUserCommand(model.Email, model.Username, model.Password);

            var response = await _userService.RegisterUser(GlobalConstants.ApiEndpoints.UsersEndpoint, request, string.Empty);

            if (response != null && !response.IsSuccess)
            {
                foreach (var error in response.ErrorMessages)
                    ModelState.AddModelError(string.Empty, error);

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
