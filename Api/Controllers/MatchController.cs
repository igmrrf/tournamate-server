using MediatR;
using Microsoft.AspNetCore.Mvc;
using static UseCase.Queries.Matches.MatchesInTournamentRound;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController(IMediator mediator) : ControllerBase
    {
        //tournament must have start first before the system can display whos playing against who
        //but number of registered team and players should be visible brofre a tournament start
        [HttpGet("GetMatchesInRound/{tournamentId}")]
        public async Task<IActionResult> GetTournamentDetails(Guid tournamentId, int round, CancellationToken cancellationToken)
        {
            var request = new GetMatchesInRoundHandler(tournamentId, round);
            var result = await mediator.Send(request);
            return Ok(result);
        }
    }
}
