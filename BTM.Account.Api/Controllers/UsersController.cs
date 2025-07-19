using System.Text.Json;
using BTM.Account.Api.Models.In;
using BTM.Account.Application.Results;
using BTM.Account.Application.Users.GetUser;
using BTM.Account.Application.Users.RegisterUser;
using Duende.IdentityServer.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BTM.Account.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class UsersController : ControllerBase
  {
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
      _mediator = mediator;
    }
    // GET: api/<UsersController>
    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Get(string id)
    {
      //validate if user is authenticated
      if (!User.Identity.IsAuthenticated)
        return Unauthorized(Result.FailureResult(new List<string> { "User is not authenticated." }));

      //validate if user id is valid
      if (!Guid.TryParse(id, out var userId))
        return BadRequest(Result.FailureResult(new List<string> { "Invalid User Id." }));

      var query = new GetUserQuery(Guid.Parse(id.ToString()));

      //todo: refactor without using MediatR
      var result = await _mediator.Send(query);

      //validate if result is successful
      if (!result.IsSuccess)
      {
        return BadRequest(Result.FailureResult(result.ErrorMessages));
      }

      return Ok(result.Data);
    }

    // POST api/<UsersController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UserRegisterRequest request, CancellationToken cancellationToken)
    {
      var command = new RegisterUserCommand(
                          request.Email,
                          request.Username,
                          request.Password);

      var result = await _mediator.Send(command, cancellationToken);

      if (!result.IsSuccess)
      {
        return BadRequest(Result.FailureResult(result.ErrorMessages));
      }

      return Created();
    }

    [HttpPost("request-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RequestToken()
    {
      // IdentityServer token endpoint
      var tokenEndpoint = "https://localhost:5001/connect/token";
      var clientId = "swagger";
      var clientSecret = "secret";
      var scope = "AccountAPI.fullaccess";

      using var httpClient = new HttpClient();

      var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);
      request.Content = new FormUrlEncodedContent(new[]
      {
        new KeyValuePair<string, string>("grant_type", "client_credentials"),
        new KeyValuePair<string, string>("client_id", clientId),
        new KeyValuePair<string, string>("client_secret", clientSecret),
        new KeyValuePair<string, string>("scope", scope)
    });

      var response = await httpClient.SendAsync(request);
      var content = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
      {
        return StatusCode((int)response.StatusCode, content);
      }

      // Optionally, parse and return just the access_token
      var json = JsonDocument.Parse(content);
      if (json.RootElement.TryGetProperty("access_token", out var token))
      {
        return Ok(new { access_token = token.GetString() });
      }

      return BadRequest("Token not found in response.");
    }

    // PUT api/<UsersController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<UsersController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
