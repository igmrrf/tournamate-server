using System.Globalization;
using System.Net;
using System.Text;
using Domain.Entities;
using Domain.ValueObject;
using MediatR;
using UseCase.Contracts.Repositories;
using UseCase.Contracts.Services;
using UseCase.Exceptions;
using static UseCase.Commands.TournamentCommand.CreatTournament1.Handler;

namespace UseCase.Commands.TournamentCommand
{
    public class CreatTournament1
    {
        public record TournamentCommand : IRequest<CreateTournamentResponse>
        {
            public string Name { get; private set; } = default!;
            public string Information { get; private set; } = default!;
            public string SportName { get; private set; }
            public string StartDate { get; private set; }
            public string EndDate { get; private set; }
            public string EndTime { get; private set; }
            public string StartTime { get; private set; }
            public int? CheckInDuration { get; private set; }
            public bool IsPrivate { get; private set; }
            public string? TounamentThumbnail {  get; private set; }
        }

        
        public class Handler(ITournamentRepository tournamentRepository, IUnitOfWork unitOfWork, IUserService userService) : IRequestHandler<TournamentCommand, CreateTournamentResponse>
        {
            public async Task<CreateTournamentResponse> Handle(TournamentCommand command, CancellationToken cancellationToken)
            {
                // Get the logged-in user
                var user = await userService.LoggedInUser() ?? throw new NullReferenceException($"User not found.");

                var isNameExist = await tournamentRepository.IsExistsAsync(t => t.TournamentInfo.Name == command.Name 
                    && t.UserId == user.Id);
                if (isNameExist)
                {
                    throw new UseCaseException($"Tournament with name {command.Name} already exists.", "NameAlreadyExists", (int)HttpStatusCode.Conflict);
                }

                // Parse the date and time
                DateTime dateStart = DateTime.ParseExact(command.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                TimeSpan timeStart = TimeSpan.Parse(command.StartTime);
                // Combine date and time
                DateTime combinedStartDateTime = dateStart.Add(timeStart);

                if(combinedStartDateTime < DateTime.UtcNow)
                {
                    throw new UseCaseException($"Start date and time cannot be in the past.", "InvalidStartDate", (int)HttpStatusCode.BadRequest);
                }

                DateTime endDate = DateTime.ParseExact(command.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                TimeSpan startTime = TimeSpan.Parse(command.StartTime);
                DateTime combinedEndDateTime = endDate.Add(startTime);



                var tournamentInfo = new TournamentInfo(
                    command.Name,
                    command.Information,
                    command.SportName,
                    combinedStartDateTime,
                    combinedEndDateTime,
                    command.CheckInDuration,
                    command.TounamentThumbnail,
                    command.IsPrivate
                );

                var code = GenerateInvitationCode(command.Name);
                
                var tournament = new Tournament(tournamentInfo, user.Id, code);

                await tournamentRepository.CreateAsync(tournament);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                return new CreateTournamentResponse
                {
                    code = code,
                    TournamentId = tournament.Id
                };;
            }

            private string GenerateInvitationCode(string tournamentName)
            {
                if (string.IsNullOrWhiteSpace(tournamentName))
                    throw new ArgumentException("Tournament name cannot be null or empty.");

                string prefix = tournamentName.Length >= 4 ? tournamentName.Substring(0, 4) : tournamentName;

                prefix = prefix.ToUpper();
                var random = new Random();
                string digits = "";
                for (int i = 0; i < 4; i++)
                {
                    digits += random.Next(0, 10).ToString();
                }

                const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                var sb = new StringBuilder();
                for (int i = 0; i < 4; i++)
                {
                    sb.Append(letters[random.Next(letters.Length)]);
                }

                string invitationCode = $"{prefix}-{digits}{sb}";
                return invitationCode;
            }

            
            
        }

        public record CreateTournamentResponse
        {
            public string code { get; set; }
            public Guid TournamentId { get; set; }
        }
    }
}
