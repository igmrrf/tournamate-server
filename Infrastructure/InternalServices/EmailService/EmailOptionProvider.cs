

namespace Infrastructure.InternalServices.EmailService
{
    public class EmailOptionProvider
    {
        public string SmtpServer { get; set; } = default!;
        public string UserName { get; set; } = Environment.GetEnvironmentVariable("EMAIL_USERNAME");
        public string Password { get; set; } = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
        public string SenderEmail { get; set; } = Environment.GetEnvironmentVariable("EMAIL_SENDER");
        public int SmtpPort { get; set; } = default!;
    }
}
