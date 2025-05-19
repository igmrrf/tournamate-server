

namespace Domain.ValueObject
{
    public class TournamentInfo
    {
        public string Name { get; private set; } = default!;
        public string Information { get; private set; } = default!;
        public string SportName { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public int? CheckInDuration { get; private set; }
        public DateTime DurationTime { get; private set; } 
        public bool IsPrivate { get; private set; }
        public string? TounamentThumbnail {  get; private set; }

        public TournamentInfo(string name, string information, string sportName, DateTime startDate, DateTime endDate, 
             int? checkInDuration, string? tounamentThumbnail, bool isPrivate )
        {
            Name = name;
            Information = information;
            SportName = sportName;
            StartDate = startDate;
            EndDate = endDate;
            CheckInDuration = checkInDuration;
            TounamentThumbnail = tounamentThumbnail;
            IsPrivate = isPrivate;
            if(CheckInDuration.HasValue)
            {
                DurationTime = startDate.AddMinutes(checkInDuration.Value);
            }
            else
            {
                DurationTime = startDate;
            }
        }

        public void UpdateTournamentInfo(string? name, string? information, string? sportName, DateTime? startDate, DateTime? endDate,
             int? checkInDuration, string? tounamentThumbnail, bool? isPrivate)
        {
            Name = name ?? Name;
            Information = information ?? Information;
            SportName = sportName ?? SportName;
            StartDate = (DateTime)(startDate.HasValue ? startDate : StartDate);
            EndDate = (DateTime)(endDate.HasValue ? endDate : EndDate);
            CheckInDuration = checkInDuration.HasValue ? checkInDuration : CheckInDuration;
            TounamentThumbnail = tounamentThumbnail ?? TounamentThumbnail;
            IsPrivate = (bool)(isPrivate.HasValue ? isPrivate : IsPrivate);
        }
    }
}