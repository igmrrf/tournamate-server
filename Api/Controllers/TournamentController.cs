
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Contracts.Services;
using static UseCase.Commands.InvitationCommand.ManagerAcceptInvitation;
using static UseCase.Commands.InvitationCommand.PlayerAcceptInvitation;
using static UseCase.Commands.TournamentCommand.CreatTournament1;
using static UseCase.Commands.TournamentCommand.DeleteTournament;
using static UseCase.Commands.TournamentCommand.EditDraftTournament;
using static UseCase.Commands.TournamentCommand.PublishTournament;
using static UseCase.Commands.TournamentCommand.SaveToDraft;
using static UseCase.Commands.TournamentCommand.StartTournament;
using static UseCase.Commands.TournamentCommand.UpdateTournament;
using static UseCase.Queries.Tournament.GetDraftTournamentById;
using static UseCase.Queries.Tournament.GetTournamentByCode;
using static UseCase.Queries.Tournament.GetTournamentId;
using static UseCase.Queries.Tournament.UserTournament;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TournamentController(IMediator mediator) : ControllerBase
    {
        [Authorize]
        [HttpPost("CreateTournament/step1")]
        public async Task<IActionResult> TournamentStep1([FromBody] TournamentCommand request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Input Invalid");
            }

            var response = await mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("CreateTournament/step2")]
        public async Task<IActionResult> CreateTournamentStep2([FromBody] UpdateTournamentCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(request, cancellationToken);
            return Ok();
        }

        //if tournament time already begin edit from the frontend the button should not show
        [Authorize]
        [HttpPost("EditTournament/step1")]
        public async Task<IActionResult> EditDraftTournament([FromBody] EditCommand request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("EditTournament/step2")]
        public async Task<IActionResult> EditTournamentStep2([FromBody] UpdateTournamentCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(request, cancellationToken);
            return Ok();
        }

        [Authorize]
        [HttpPut("PublishTournament/{tournamentId}/{url}")]
        public async Task<IActionResult> TournamentStep2([FromRoute] string url, Guid tournamentId, CancellationToken cancellationToken)
        {
            var request = new PublishTournamentCommand(tournamentId, url);
            await mediator.Send(request, cancellationToken);
            return Ok();
        }

        [Authorize]
        [HttpPut("DeleteTournament/{tournamentId}")]
        public async Task<IActionResult> DeleteTournament([FromRoute] Guid tournamentId, CancellationToken cancellationToken)
        {
            var request = new DeleteTournamentCommand(tournamentId);
            await mediator.Send(request, cancellationToken);
            return Ok();
        }

        [Authorize]
        [HttpPut("SaveTournamentToDraft/{tournamentId}")]
        public async Task<IActionResult> DraftTournament([FromRoute] Guid tournamentId, CancellationToken cancellationToken)
        {
            var request = new SaveToDraftCommand(tournamentId);
            await mediator.Send(request, cancellationToken);
            return Ok();
        }

        [Authorize]
        [HttpPut("StartTournament/{tournamentId}")]
        public async Task<IActionResult> StartTournament([FromRoute] Guid tournamentId, CancellationToken cancellationToken)
        {
            var request = new StartTournamentCommand(tournamentId);
            await mediator.Send(request, cancellationToken);
            return Ok();
        }

        [Authorize]
        [HttpGet("GetTournamentById/{tournamentId}")]
        public async Task<IActionResult> GetTournamentById([FromRoute] Guid tournamentId, CancellationToken cancellationToken)
        {
            var request = new GetTournamentCommand(tournamentId);
            var response = await mediator.Send(request, cancellationToken);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest();
        }

        [Authorize]
        [HttpGet("GetDraftTournamentById/{tournamentId}")]
        public async Task<IActionResult> GetDraftTournamentById([FromRoute] Guid tournamentId, CancellationToken cancellationToken)
        {
            var request = new GetDraftTournamentCommand(tournamentId);
            var response = await mediator.Send(request, cancellationToken);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPost("PlayerAcceptInvitation/{code}")]
        public async Task<IActionResult> PlayerAcceptInvitation([FromBody] PlayerAcceptInvitationCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(request, cancellationToken);
            return Ok();
        }

        [Authorize]
        [HttpPost("ManagerAcceptInvitation")]
        public async Task<IActionResult> ManagerAcceptInvitation([FromBody] ManagerAcceptInviteCOmmand request, CancellationToken cancellationToken)
        {
            await mediator.Send(request, cancellationToken);
            return Ok();
        }


        [Authorize]
        [HttpGet("SearchTournamentByCode/{code}")]
        public async Task<IActionResult> SearchTournamentByCode([FromRoute] string code, CancellationToken cancellationToken)
        {
            var request = new GetTournamentByCodeCommand(code);
            var response = await mediator.Send(request, cancellationToken);
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest();
        }

        //[Authorize(Policy = "RequireAdmin")]
        [Authorize]
        [HttpGet("AllTournamentCreatedByUSer{status}")]
        public async Task<IActionResult> AllTournamentCreatedByUSer([FromRoute] int status, CancellationToken cancellationToken)
        {
            var request = new GetUserTournament(status);
            var response = await mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}