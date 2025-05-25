

namespace Infrastructure.InternalServices.EmailService
{
    public class EmailOptionProvider
    {
        public string SmtpHost { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string SenderEmail { get; set; } = default!;
        public int SmtpPort { get; set; } = default!;
    }
}
