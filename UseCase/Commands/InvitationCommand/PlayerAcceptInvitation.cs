

using System.Net;
using System.Text.RegularExpressions;
using Domain.Aggregate.TournamentAggregate;
using Domain.Enum;
using Domain.Shared.Enum;
using MediatR;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.Exceptions;
using static UseCase.Services.LoginUser;

namespace UseCase.Commands.InvitationCommand
{
    public class PlayerAcceptInvitation
    {
        public record PlayerAcceptInvitationCommand : IRequest
        {
            public string Name { get; set; }
            public string PlayerPosition { get; set; }
            public string JerseyNumber { get; set; }
            public string Code { get; set; }
            public Guid TournamentId { get; set; }

        }

        public class Handler(ITournamentRepository tournamentRepository, IUnitOfWork unitOfWork, IUserService userService, ITeamRepository teamRepository) : IRequestHandler<PlayerAcceptInvitationCommand>
        {
            public async Task Handle(PlayerAcceptInvitationCommand request, CancellationToken cancellationToken)
            {
                var getTournament = await tournamentRepository.GetAsync(t => t.Id == request.TournamentId 
                && t.InvitationCode == ExtractInvitationCode(request.Code)) ??
                    throw new UseCaseException($"Tournament Not Found.",
                    "NotFound", (int)HttpStatusCode.NotFound);

                if (getTournament.Status == TournamentStatus.Draft)
                {
                    throw new UseCaseException($"Tournament Is Not Yet Publish.",
                    "NotYetPublish", (int)HttpStatusCode.NotFound);
                }

                if (getTournament.IsTournamentStarted)
                {
                    throw new UseCaseException($"Tournament Already Started.",
                    "AlreadyStarted", (int)HttpStatusCode.NotFound);
                }


                var user = await userService.LoggedInUser() ?? throw new NullReferenceException($"User not found.");

                if (getTournament.TournamentMode == TournamentMode.team_VS_team)
                {
                    var getTeam = await teamRepository.GetAsync(t => t.Code == ExtractInvitationCode(request.Code)
                    && t.TournamentId == request.TournamentId) ??
                        throw new UseCaseException($"Team Not Found.",
                        "NotFound", (int)HttpStatusCode.NotFound);
                    
                    var player = new Player(request.Name, request.PlayerPosition, request.JerseyNumber, user.Id, request.TournamentId, getTeam.Id);

                    var playerType = ExtractFirstPrefix(request.Code);

                    if (playerType == "First11")
                    {
                        getTournament.AddPlayerToTeam(player);
                    }

                    if (playerType == "Subtitute")
                    {
                        getTournament.AddSubPlayerToTeam(player);
                    }

                }

                if(getTournament.TournamentMode == TournamentMode.player_VS_player)
                {
                    var getTeam = await teamRepository.GetAsync(t => t.Code == request.Code
                    && t.TournamentId == request.TournamentId) ??
                        throw new UseCaseException($"Team Not Found.",
                        "NotFound", (int)HttpStatusCode.NotFound);

                    var player = new Player(request.Name, request.PlayerPosition, request.JerseyNumber, user.Id, request.TournamentId, getTeam.Id);
                    getTournament.AddPlayer(player);
                }

                getTournament.AddParticipant(user.Id, Role.Player);
                await unitOfWork.SaveChangesAsync(cancellationToken);

            }

            private string? ExtractInvitationCode(string code)
            {
                // Define the regex pattern to match the invitation code format
                string pattern = @"([^-]+-\d{4}[A-Z]{4})";

                // Use Regex to find the invitation code in the input string
                System.Text.RegularExpressions.Match match = Regex.Match(code, pattern);

                // If a match is found, return the invitation code
                if (match.Success)
                {
                    return match.Value;
                }

                return null; 
            }

            private string? ExtractFirstPrefix(string invitationCode)
            {
                if (string.IsNullOrWhiteSpace(invitationCode))
                    return null;
                // Split by hyphen 
                var parts = invitationCode.Split('-');
                // Return first part if exists
                if (parts.Length > 0)
                    return parts[0];
                return null;
            }
        }
    }
}
