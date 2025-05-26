

using System.Net;
using Domain.Aggregate.TournamentAggregate;
using Domain.Enum;
using Domain.Shared.Enum;
using MediatR;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.Exceptions;

namespace UseCase.Commands.TournamentCommand
{
    public class UpdateTournament
    {
        public record UpdateTournamentCommand : IRequest
        {
            public Guid TournamentId {get; set;}
            public int TournamentMode { get;  set; }
            public int TournamentType { get;  set; }
            public int NoOfPlayers { get; set; }
            public int NoOfTeams { get; set; }
            public int? NoOfSubPlayers { get; set; }
            public List<permissionCommand?> Permissions { get; set; } = new();
        }

        public record permissionCommand
        {
            public string UserName { get; set; } = default!;
            public bool CanUpdateScore { get;  set; }
            public bool CanRecordFoul { get;  set; }
            public bool CanMakeSubstitutions { get;  set; }
            public bool CanUpdateTimeStamp { get;  set; }
        }

        public class Handler(ITournamentRepository tournamentRepository, IUserRepository userRepository ,IUnitOfWork unitOfWork, IUserService userService) : IRequestHandler<UpdateTournamentCommand>
        {
            public async Task Handle(UpdateTournamentCommand command, CancellationToken cancellationToken)
            {
                // Get the logged-in user
                var user = await userService.LoggedInUser() ?? throw new NullReferenceException($"User not found.");

                var getTournament = await tournamentRepository.GetAsync( t => t.Id == command.TournamentId
                                    && t.UserId == user.Id) ?? 
                    throw new UseCaseException($"Tournament Not Found.", 
                    "NotFound", (int)HttpStatusCode.NotFound);

                if (command.Permissions.Any())
                {
                    var refreeIds = new List<Guid>();
                    var userNames = command.Permissions.Select(p => p.UserName).ToList();

                    var users = await userRepository.GetManyAsync(u => userNames.Contains(u.UserName));

                    foreach (var userName in userNames)
                    {
                        var userref = users.FirstOrDefault(u => string.Equals(u.UserName, userName, StringComparison.OrdinalIgnoreCase));
                        if (userref is null)
                        {
                            throw new UseCaseException($"User  Name {userName} doesn't exist.", "NotFound", (int)HttpStatusCode.NotFound);
                        }
                        refreeIds.Add(user.Id);
                    }

                    foreach (var id in refreeIds.Distinct())
                    {
                        getTournament.AddParticipant(id, Role.Referee);
                    }

                    foreach (var userPermit in command.Permissions)
                    {
                        var userId = users.First(u => string.Equals(u.UserName, userPermit.UserName, StringComparison.OrdinalIgnoreCase)).Id;
                        var permit = new Permission(
                            command.TournamentId,
                            userPermit.CanUpdateScore,
                            userPermit.CanRecordFoul,
                            userPermit.CanMakeSubstitutions,
                            userPermit.CanUpdateTimeStamp,
                            userId
                        );

                        getTournament.AssignPermission(userId, permit);
                    }
                }



                getTournament.UpdateTournament((TournamentMode)command.TournamentMode, (TournamentType)command.TournamentType, command.NoOfPlayers,
                                                command.NoOfTeams, command.NoOfSubPlayers);

                getTournament.AddParticipant(user.Id, Role.Admin);
                await tournamentRepository.UpdateTournament(getTournament);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}