namespace Infrastructure.InternalServices.EmailService
{
    public class EmailResponseModel
    {
        public string ToEmail { get; set; } = default!;
        public string ToName { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public string HtmlContent { get; set; } = default!;
    }
}
