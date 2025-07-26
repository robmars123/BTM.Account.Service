using System.Diagnostics;
using System.Security.Claims;
using BTM.Account.Application.Abstractions;
using BTM.Account.Application.DTOs;
using BTM.Account.Application.Results;
using BTM.Account.Application.Users.RegisterUser;
using BTM.Account.MVC.UI.Controllers.Base;
using BTM.Account.MVC.UI.Models.Requests;
using BTM.Account.MVC.UI.Models.Results;
using BTM.Account.Shared.Common;
using Duende.IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

      if (!response.IsSuccess)
      {
        foreach (var error in response.ErrorMessages)
          ModelState.AddModelError(string.Empty, error);

        return View();
      }

      RegisterRequest.Reset();
      return RedirectToAction("Login", "Account");
    }

    public async Task<IActionResult> Login(string returnUrl = "/")
    {
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

      var redirectUri = Url.IsLocalUrl(returnUrl) ? returnUrl : "/";

      return Challenge(new AuthenticationProperties
      {
        RedirectUri = redirectUri
      }, OpenIdConnectDefaults.AuthenticationScheme);
    }

    [Authorize]
    public async Task<IActionResult> LogoutAsync()
    {
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

      return SignOut(new AuthenticationProperties
      {
        RedirectUri = "/",
      }, OpenIdConnectDefaults.AuthenticationScheme);
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
