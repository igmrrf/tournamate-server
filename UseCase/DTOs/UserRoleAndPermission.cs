

namespace UseCase.DTOs
{
    public class UserRoleAndPermission
    {
        public bool CanUpdateScore { get;  set; }
        public bool CanRecordFoul { get;  set; }
        public bool CanMakeSubstitutions { get; set; }
        public bool CanUpdateTimeStamp { get;  set; }
        public string Role { get; set; }
    }
}
