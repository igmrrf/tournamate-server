

using Domain.Entities;
using System.Globalization;
using System.Net;
using MediatR;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.Exceptions;
using static UseCase.Commands.TournamentCommand.CreatTournament1;

namespace UseCase.Commands.TournamentCommand
{
    public class EditDraftTournament
    {
        public record EditCommand : IRequest<CreateTournamentResponse>
        {
            public string Name { get;  set; } = default!;
            public string Information { get;  set; } = default!;
            public string SportName { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string EndTime { get; set; }
            public string StartTime { get; set; }
            public int? CheckInDuration { get; set; }
            public bool IsPrivate { get; set; }
            public string? TounamentThumbnail { get; set; }
            public Guid TournamentId { get; set; }
        }

        public class Handler(ITournamentRepository tournamentRepository, IUnitOfWork unitOfWork, IUserService userService) : IRequestHandler<EditCommand, CreateTournamentResponse>
        {
            public async Task<CreateTournamentResponse> Handle(EditCommand command, CancellationToken cancellationToken)
            {
                // Get the logged-in user
                var user = await userService.LoggedInUser() ?? throw new NullReferenceException($"User not found.");

                var getTournament = await tournamentRepository.GetAsync(t => t.Id == command.TournamentId
                  && t.UserId == user.Id) 
                    ?? throw new UseCaseException($"Tournament Not Found.", "NotFound", (int)HttpStatusCode.NotFound);
               

                if (command.Name != null) 
                {
                    var isNameExist = await tournamentRepository.IsExistsAsync(t => t.TournamentInfo.Name == command.Name
                    && t.UserId == user.Id);
                    if (isNameExist)
                    {
                        throw new UseCaseException($"Tournament with name {command.Name} already exists.", "NameAlreadyExists", (int)HttpStatusCode.Conflict);
                    }
                }


                // Parse the date and time
                DateTime dateStart = DateTime.ParseExact(command.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                TimeSpan timeStart = TimeSpan.Parse(command.StartTime);
                // Combine date and time
                DateTime combinedStartDateTime = dateStart.Add(timeStart);

                DateTime endDate = DateTime.ParseExact(command.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                TimeSpan startTime = TimeSpan.Parse(command.StartTime);
                DateTime combinedEndDateTime = endDate.Add(startTime);

                getTournament.TournamentInfo.UpdateTournamentInfo(command.Name,
                command.Information,
                command.SportName,
                combinedStartDateTime,
                combinedEndDateTime,
                command.CheckInDuration,
                command.TounamentThumbnail,
                command.IsPrivate);


                await unitOfWork.SaveChangesAsync(cancellationToken);
                return new CreateTournamentResponse
                {
                    code = getTournament.InvitationCode,
                    TournamentId = command.TournamentId,
                }; ;
            }

          

        }
    }
}
