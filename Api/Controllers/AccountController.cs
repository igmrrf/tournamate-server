
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static UseCase.Commands.UserCommand.UserSignUp;
using static UseCase.Queries.User.UserRole;
using static UseCase.Services.ChangePassword;
using static UseCase.Services.ForgotPassword;
using static UseCase.Services.LoginUser;
using static UseCase.Services.ResetPassword;
using static UseCase.Services.VerifyEmail;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IMediator mediator) : ControllerBase
    {
        [HttpPost("Sign-Up")]
        public async Task<IActionResult> ApplicantSignUp([FromBody] UserSignUpCommand request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Input Invalid");
            }

            var response = await mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginRequestModel request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Input Invalid");
            }

            var response = await mediator.Send(request, cancellationToken);
            return Ok(response);
        }


        [HttpPost("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Input");
            }

            await mediator.Send(request, cancellationToken);
            return Ok();
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Input");
            }

            await mediator.Send(request, cancellationToken);
            return Ok();
        }

        [HttpPost("Forgot-Password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Input");
            }
            await mediator.Send(request, cancellationToken);
            return Ok();
        }

        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Input");
            }
            await mediator.Send(request, cancellationToken);
            return Ok();
        }

        [HttpGet("{tournamentId}/my-role")]
        public async Task<IActionResult> GetUserRole([FromRoute] Guid tournamentId)
        {
            var request = new UserRoleCommand(tournamentId);
            var result = await mediator.Send(request);
            return Ok(result);
        }
    }
}
