using BTM.Account.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BTM.Account.MVC.UI.Controllers.Base
{
    public class BaseController : Controller
    {
        private readonly ITokenService _tokenService;

        public BaseController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        protected async Task<string> GetAccessTokenAsync()
        {
            return await _tokenService.GetAccessTokenAsync();
        }
    }
}
