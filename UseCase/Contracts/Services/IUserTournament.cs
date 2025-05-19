
using UseCase.DTOs;
using static UseCase.Queries.Tournament.GetTournamentId;
namespace UseCase.Contracts.Services
{
    public interface IUserTournament
    {
        Task<ICollection<BaseResponse<TournamentResponse>>> AllTornamentByUser();
        Task<ICollection<BaseResponse<TournamentResponse>>> UserTournamentByStatus(int status);
    }
}