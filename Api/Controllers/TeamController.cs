using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCase.Commands.TeamCommand;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController(IMediator mediator) : ControllerBase
    {
        [HttpPut("Remove-Player/{ playerId}")]
        public async Task<IActionResult> RemovePlayer([FromRoute] Guid playerId, CancellationToken cancellationToken)
        {
            var request = new RemovePlayer.RemovePlayerCommand(playerId);
            await mediator.Send(request, cancellationToken);
            return Ok($"Player removed from tournament.");
        }

        [HttpPut("Remove-Team/{ teamId}")]
        public async Task<IActionResult> RemoveTeam([FromRoute] Guid teamId, CancellationToken cancellationToken)
        {
            var request = new RemoveTeam.RemoveTeamCommand(teamId);
            await mediator.Send(request, cancellationToken);
            return Ok($"Team removed from tournament.");
        }

        [HttpPut("RemovePlayer-Team/{ teamId}/{ playerId}")]
        public async Task<IActionResult> RemovePlayerTeam([FromRoute] Guid teamId, Guid playerId, CancellationToken cancellationToken)
        {
            var request = new RemovePlayerFromTeam.RemovePlayerTeamCommand(teamId, playerId);
            await mediator.Send(request, cancellationToken);
            return Ok($"Player removed from Team.");
        }

        [HttpPut("RemoveSubPlayer-Team/{ teamId}/{ playerId}")]
        public async Task<IActionResult> RemoveSubPlayer([FromRoute] Guid teamId, Guid playerId, CancellationToken cancellationToken)
        {
            var request = new RemoveSubPlayer.RemoveSubPlayerTeamCommand(teamId, playerId);
            await mediator.Send(request, cancellationToken);
            return Ok($"Player removed from Team.");
        }

        [HttpPost("Update-Player")]
        public async Task<IActionResult> UpdatePlayer([FromBody] UpdatePlayer.UpdatePlayerCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(request, cancellationToken);
            return Ok($"Player updated successfully.");
        }

        [HttpPost("Update-Team")]
        public async Task<IActionResult> UpdateTeam([FromBody] UpdateTeam.UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(request, cancellationToken);
            return Ok($"Team updated successfully.");
        }
    }
}
