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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BTM.ApiClients;
using BTM.ApiClients.ProductService;
using BTM.Account.ApiClient.Abstractions;
using BTM.Account.Infrastructure.Gateways;

namespace BTM.Account.MVC.Client.Controllers
{
  public class AccountController : BaseController
  {
    private readonly IUserGateway _userGateway;
    private readonly IProductApiClient _productApiClient;

    public AccountController(IHttpContextAccessor httpContextAccessor,
                                 ITokenService tokenService,
                                 IUserGateway userGateway,
                                 IProductApiClient productApiClient) : base(tokenService)
    {
      _userGateway = userGateway;
      _productApiClient = productApiClient;
    }
    [HttpGet]
    public async Task<IActionResult> IndexAsync()
    {
      //check if user is authenticated
      if (!User.Identity.IsAuthenticated)
        return Challenge();// triggers re-authentication

      //get user id from claims
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (userId == null)
        return View("Error"); // better UX

      //get access token to access API
      var accessToken = await GetAccessTokenAsync();

      if (string.IsNullOrEmpty(accessToken))
        return View();

      //sample call to product API using APIClient facade
      //var productClient = await _productApiClient.GetAsync();

      Result<UserDTO> user = await _userGateway.GetUserAsync(userId, accessToken);

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
    public async Task<IActionResult> RegisterAsync(RegisterRequest model)
    {
      var request = new RegisterUserCommand(model.Email, model.Username, model.Password);

      var response = await _userGateway.RegisterUser(GlobalConstants.ApiEndpoints.UsersEndpoint, request, string.Empty);

      if (!response.IsSuccess)
      {
        foreach (var error in response.ErrorMessages)
          ModelState.AddModelError(string.Empty, error);

        return View();
      }

      RegisterRequest.Reset();
      return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    public async Task<IActionResult> LoginAsync(string returnUrl = "/")
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
