using Microsoft.Extensions.Options;
using UseCase.Contracts.InternalServices;
using UseCase.Contracts.Services;
using MimeKit;
using Microsoft.AspNetCore.Routing;

namespace Infrastructure.InternalServices.EmailService
{ 
    public class EmailProvider : IEmailProvider
    {
        public EmailOptionProvider Option { get; }
        public EmailProvider(
            IOptionsSnapshot<EmailOptionProvider> options )
        {
            Option = options.Value;
        }
        

        private async Task SendEmailAsync(EmailResponseModel mailRequest)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("TOURNAMATE", Option.SenderEmail));
            message.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            message.Subject = mailRequest.Subject;

            var body = new TextPart("html")
            {
                Text = mailRequest.HtmlContent,
            };
            message.Body = body;

            using var client = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                await client.ConnectAsync(Option.SmtpServer, Option.SmtpPort, false);
                await client.AuthenticateAsync(Option.UserName, Option.Password);
                await client.SendAsync(message);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to send email.", ex);
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }

        public async Task SendEmailVerificationMessage(string name, string email, string callbackUrl)
        {
            string subject = "Email Verification";
            string html = $@"
    <body>
        <div>
            <h1>TOURNAMATE</h1>
            <p3>Hello <strong>{name}</strong>,</p3>
            <p>Verify your Email with the link below</p>
            <a href=""{callbackUrl}"">Confirm your mail first :)</a>
        </div>
        <div>
            <p>Best Regard</p>
        </div>
    </body>";

            var model = new EmailResponseModel
            {
                ToName = name,
                HtmlContent = html,
                Subject = subject,
                ToEmail = email,
            };
            await SendEmailAsync(model);
        }

        public async Task SendForgotPasswordLink(string name, string email, string callbackUrl)
        { 
            string subject = "Reset Password";
            string html = $@"
    <body>
        <div>
            <h1>TOURNAMATE</h1>
            <p3>Hello {name}</strong>,</p3>
            <p>Reset your Password with the link below</p>
            <a href=""{callbackUrl}"">Reset Password :)</a>
        </div>
        <div>
            <p>Best Regard</p>
        </div>
    </body>";

            var model = new EmailResponseModel
            {
                ToName = name,
                HtmlContent = html,
                Subject = subject,
                ToEmail = email,
            };
            await SendEmailAsync(model);
        }
    }
}
