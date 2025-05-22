
using Domain.Aggregate.TournamentAggregate;
using Domain.Enum;
using Domain.Shared.Entities;
using Domain.Shared.Enum;
using Domain.Shared.Exceptions;
using Domain.ValueObject;
using Match = Domain.Aggregate.TournamentAggregate.Match;

namespace Domain.Entities
{
    public class Tournament : BaseEntity
    {

        
        //public List<Permission?> Permissions { get; private set; } = new();

        //my thought process about the implementation is that once a tournament begins nobody and register for the tournament again which i av already implemented.the system now randomly selected team or player to player against them selfes depending on the tournament type, a user who is then given the permission to edit or update the match can now set the start time of each match in a tournament, he will also update other details as the tournament goes on like fouls, scores and substitution, whichever of the team that wins the system should move it to the next round till it get to final. also note that we also have knockout and home and away matches
        public List<Match> Matches { get; private set; } = new();
        public int CurrentPlayerCount { get; private set; }
        public int CurrentTeamCount { get; private set; }
        private readonly List<Team> _teams = new();
        private readonly List<Player> _players = new();
        public IReadOnlyCollection<Player> Players => _players.AsReadOnly();
        public IReadOnlyCollection<Team> Teams => _teams.AsReadOnly();
        public Guid UserId { get; private set; }
        public TournamentInfo TournamentInfo { get; private set; } = default!;
        public TournamentStatus Status { get; private set; }
        public string InvitationCode { get; private set; } = default!;
        public TournamentMode TournamentMode { get; private set; }
        public TournamentType TournamentType { get; private set; }
        public int NoOfPlayers { get; private set; }
        public int NoOfTeams { get; private set; }
        public int? NoOfSubPlayers { get; set; }
        public bool IsTournamentStarted  => DateTime.UtcNow >= TournamentInfo.StartDate;
        private readonly List<TournamentRound> _rounds = new();
        public IReadOnlyCollection<TournamentRound> Rounds => _rounds.AsReadOnly();
        public List<TournamentRole> Participants { get; set; } = new();

        public void StartTournament()
        {
            if (Status != TournamentStatus.Upcoming)
                throw new DomainException("Only published tournaments can be started");

            if (TournamentMode == TournamentMode.TeamVsTeam && CurrentTeamCount < 2)
                throw new DomainException("Need at least 2 teams to start tournament");

            if (TournamentMode == TournamentMode.PlayerVsPlayer && CurrentPlayerCount < 2)
                throw new DomainException("Need at least 2 players to start tournament");

            GenerateTournamentMatches();
            Status = TournamentStatus.Ongoing;
            //AddDomainEvent(new TournamentStartedEvent(Id));
        }   
        public void GenerateTournamentMatches()
        {
            if (Status == TournamentStatus.Draft)
                throw new DomainException("Tournament must be published to generate matches");
            if (Teams.Count < 2 || Players.Count < 2)
                throw new DomainException("Need at least 2 teams or players to generate matches");

            _rounds.Clear();
            
            if (TournamentType == TournamentType.Home_AND_Away && TournamentMode == TournamentMode.TeamVsTeam)
            {
                GenerateHomeAwayMatches();
            }
            else  
            {
                GenerateKnockoutMatches();
            }
        }

        public void MatchTime(DateTime startTime, DateTime endTime, bool hasMatchbegan, 
            bool hasMatchEnded, WhoWon whoWon, Guid userId, Guid matchId)
        {
            if (!IsTournamentStarted)
                throw new DomainException("Tournament hasn't started yet");

            var match = _rounds.SelectMany(r => r.Matches)
                .FirstOrDefault(m => m.Id == matchId)
                ?? throw new DomainException("Match not found");

            if (match.MatchStatus == MatchStatus.Cancelled)
                throw new DomainException("Only scheduled matches can be assigned dates");

            var time = new MatchTimeStamp(matchId, startTime, endTime, hasMatchbegan, hasMatchEnded, whoWon, userId);

            match.UpdateMatchTimeStamp(time);
        }

        //public void MakeSubstitution(TeamId teamId, PlayerId playerOut, PlayerId playerIn)
        //{
        //    if (!IsTournamentStarted)
        //        throw new DomainException("Tournament hasn't started");

        //    var team = _teams.FirstOrDefault(t => t.Id == teamId)
        //        ?? throw new DomainException("Team not found");

        //    team.MakeSubstitution(playerOut, playerIn, NoOfSubPlayers);
        //}

        private void GenerateHomeAwayMatches()
        {
            if (Teams.Count < 2)
                throw new DomainException("Need at least 2 teams for home/away matches");

            var teams = Teams.ToList();
            ShuffleTeams(teams);

            // Create round-robin schedule where each team plays every other team twice
            int numberOfRounds = (teams.Count - 1) * 2;
            int matchesPerRound = teams.Count / 2;

            for (int round = 1; round <= numberOfRounds; round++)
            {
                var tournamentRound = new TournamentRound(round);

                for (int match = 0; match < matchesPerRound; match++)
                {
                    int homeIndex = (round + match) % (teams.Count - 1);
                    int awayIndex = (teams.Count - 1 - match + round) % (teams.Count - 1);

                    // In odd rounds, swap home/away for return matches
                    if (round > numberOfRounds / 2)
                    {
                        (homeIndex, awayIndex) = (awayIndex, homeIndex);
                    }

                    // Ensure we don't get "team vs self"
                    if (homeIndex == awayIndex)
                    {
                        awayIndex = teams.Count - 1;
                    }

                    var homeTeam = teams[homeIndex];
                    var awayTeam = teams[awayIndex];

                    var tournamentMatch = new Match(
                        Id,
                        homeTeam.Id,
                        awayTeam.Id,
                        MatchStatus.Shedulled
                        );

                    tournamentRound.AddMatch(tournamentMatch);
                }

                _rounds.Add(tournamentRound);
            }
        }

        // Method to advance winners to next round
        public void AdvanceWinners(Guid matchId, Guid winnerId)
        {
            var match = _rounds.SelectMany(r => r.Matches)
            .FirstOrDefault(m => m.Id == matchId)
            ?? throw new DomainException("Match not found");
            if (match.MatchStatus != MatchStatus.Completed)
                throw new DomainException("Cannot advance unfinished match");
            var currentRound = _rounds.First(r => r.Matches.Contains(match));
            int currentRoundNumber = currentRound.RoundNumber;

            // If this is the final, mark tournament as completed
            if (currentRoundNumber == _rounds.Count && _rounds.Count > 1)
            {
                Status = TournamentStatus.Completed;
                //AddDomainEvent(new TournamentCompletedEvent(Id, winnerId));
                return;
            }

            // Get or create next round
            TournamentRound nextRound;
            if (_rounds.Count > currentRoundNumber)
            {
                nextRound = _rounds[currentRoundNumber];
            }
            else
            {
                nextRound = new TournamentRound(currentRoundNumber + 1);
                _rounds.Add(nextRound);
            }

            var nextMatch = nextRound.Matches
            .FirstOrDefault(m => m.HomeId == null || m.AwayId == null);

            if (nextMatch == null)
            {
                // Create new match in next round    an error here dont forget the status
                nextMatch = new Match(Id,
                     winnerId,
                    Guid.Empty,
                    MatchStatus.Shedulled);
                nextRound.AddMatch(nextMatch);
            }
            else
            {
                // Fill in the spot in existing match
                if (nextMatch.HomeId == null)
                {
                    nextMatch.SetHomeTeam(winnerId);
                }
                else
                {
                    nextMatch.SetAwayTeam(winnerId);
                    nextMatch.ShedulleMatch();
                }
            }
        }

        private void GenerateKnockoutMatches()
        {
            var teams = Teams.ToList();
            ShuffleTeams(teams); 

            // Handle cases where team count isn't power of 2
            int totalTeams = teams.Count;
            int nextPowerOfTwo = (int)Math.Pow(2, Math.Ceiling(Math.Log(totalTeams) / Math.Log(2)));
            int teamsNeedingBye = nextPowerOfTwo - totalTeams;

            var firstRound = new TournamentRound(1);

            // Create first round matches
            int matchIndex = 0;
            for (int i = 0; i < teams.Count - teamsNeedingBye; i += 2)
            {
                var match = new Match(Id,
                    teams[i].Id,
                    teams[i + 1].Id,
                    MatchStatus.Shedulled);

                firstRound.AddMatch(match);
                matchIndex++;
            }

            // Handle teams that get a bye
            for (int i = 0; i < teamsNeedingBye; i++)
            {
                var team = teams[teams.Count - teamsNeedingBye + i];
                firstRound.AddBye(team.Id);
            }

            _rounds.Add(firstRound);
        }

        private void ShuffleTeams(List<Team> teams)
        {
            var rng = new Random();
            int n = teams.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (teams[n], teams[k]) = (teams[k], teams[n]);
            }
        }

        public void AddParticipant(Guid userId, Role role)
        {
            Participants.Add(new TournamentRole
            {
                UserId = userId,
                TournamentId = this.Id,
                Role = role
            });
        }

        public void AssignPermission(Guid userId, Permission permission)
        {
            var participant = Participants.FirstOrDefault(p => p.UserId == userId);
            if (participant != null && !participant.Permissions.Contains(permission))
            {
                participant.Permissions.Add(permission);
            }
        }
        public void UpdateTournament(TournamentMode tournamentMode, TournamentType tournamentType, int noOfPlayers, int noOfTeams,
                             int? noOfSubPlayers)
        {
            TournamentMode = tournamentMode;
            TournamentType = tournamentType;
            NoOfPlayers = noOfPlayers > 0 ? noOfPlayers : throw new DomainException("Number of required players must be more than 0.");
            NoOfTeams = noOfTeams;
            NoOfSubPlayers = noOfSubPlayers;
        }


        // step1
        public Tournament(TournamentInfo tournamentInfo, Guid userId, string invitationCode)
        {
            TournamentInfo = tournamentInfo ?? throw new DomainException("TournamentInfo cannot be null.");
            UserId = userId;
            InvitationCode = invitationCode;
            Status = TournamentStatus.Draft;
            CurrentPlayerCount = 0;
            CurrentTeamCount = 0;
        }
        public void AddTeam(Team team)
        {
            if (CurrentTeamCount > NoOfTeams)
            {
                throw new DomainException("Tournament Team limit reached");
            }

            _teams.Add(team);
            CurrentTeamCount++;
        }
        public void AddPlayerToTeam(Player player)
        {
            _teams.FirstOrDefault(t => t.Id == player.TeamId)?.AddPlayerToTeam(player);
        }

        public void AddSubPlayerToTeam(Player player)
        {
            _teams.FirstOrDefault(t => t.Id == player.TeamId)?.AddSubPlayerToTeam(player);
        }
        public void AddPlayer(Player player)
        {
            if (TournamentMode == TournamentMode.TeamVsTeam)
            {
                throw new DomainException("Team is required in Team vs Team mode");
            }

            if (CurrentPlayerCount >= NoOfPlayers)
            {
                throw new DomainException("Tournament player limit reached");
            }

            _players.Add(player);
            CurrentPlayerCount++;
        }

        public void RemovePlayer(Guid playerId)
        {
            var player = _players.FirstOrDefault(p => p.Id == playerId);
            if (player == null) return;

            _players.Remove(player);
            CurrentPlayerCount--;
        }

        public void RemoveTeam(Guid teamId)
        {
            var team = _teams.FirstOrDefault(t => t.Id == teamId);
            if (team == null) return;

            _teams.Remove(team);
            CurrentTeamCount--;
        }

        public void RemovePlayerFromTeam(Guid PlayerId, Guid teamId)
        {
            var team = _teams.FirstOrDefault(t => t.Id == teamId);
            if (team == null) return;

            var player = team.Players.FirstOrDefault(p => p.Id == PlayerId);
            if (player == null) return;

            team.Players.Remove(player);
            CurrentPlayerCount--;
        }



        public void CancelTournament(Tournament tournament)
        {
            tournament.Status = TournamentStatus.Cancelled;
        }

        public void SaveToDraft(Tournament tournament)
        {
            tournament.Status = TournamentStatus.Draft;
        }

        public void PublishTournament(Tournament tournament)
        {
            var currentDate = DateTime.UtcNow;

            switch (currentDate)
            {
                case var currentTime when currentTime < tournament.TournamentInfo.StartDate:
                    tournament.Status = TournamentStatus.Upcoming;
                    break;
                case var currentTime when currentTime >= tournament.TournamentInfo.StartDate && currentTime <= tournament.TournamentInfo.EndDate:
                    tournament.Status = TournamentStatus.Ongoing;
                    break;
                case var currentTime when currentTime > tournament.TournamentInfo.EndDate:
                    tournament.Status = TournamentStatus.Completed;
                    break;
                default:
                    tournament.Status = TournamentStatus.Draft;
                    break;
            }
        }
    }
}
