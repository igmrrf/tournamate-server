using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        //tournament must have start first before the system can display whos playing against who
        //but number of registered team and players should be visible brofre a tournament start
        //[HttpGet("GetTournamentDetails/{tournamentId}")]
        //public Task<IActionResult> GetTournamentDetails()
        //{
        //    //Full detail including match

        //}
    }
}
