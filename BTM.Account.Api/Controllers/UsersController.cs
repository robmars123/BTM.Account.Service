using BTM.Account.Api.Models.In;
using BTM.Account.Application.Results;
using BTM.Account.Application.Users.GetUser;
using BTM.Account.Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BTM.Account.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
