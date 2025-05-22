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
<body style='font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;'>
    <div style='max-width: 600px; margin: auto; background-color: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0,0,0,0.1);'>

        <div style='text-align: center;'>
            <img src='https://cdn-icons-png.flaticon.com/512/1055/1055646.png' alt='Champion Cup' style='width: 80px; height: auto; margin-bottom: 20px;'/>
            <h1 style='color: #2c3e50;'>TOURNAMATE</h1>
        </div>

        <p style='font-size: 16px; color: #333333;'>Hello <strong>{name}</strong>,</p>
        <p style='font-size: 16px; color: #333333;'>
            Thank you for signing up. Please verify your email address by clicking the button below:
        </p>

        <div style='text-align: center; margin: 30px 0;'>
            <a href='{callbackUrl}' style='display: inline-block; padding: 12px 24px; font-size: 16px; color: #ffffff; background-color: #27ae60; text-decoration: none; border-radius: 5px;'>
                Confirm Your Email
            </a>
        </div>

        <p style='font-size: 14px; color: #666666;'>If the button above doesn't work, copy and paste this URL into your browser:</p>
        <p style='font-size: 14px; color: #666666; word-break: break-all;'>{callbackUrl}</p>

        <hr style='margin: 30px 0; border: none; border-top: 1px solid #eeeeee;'/>
        <p style='font-size: 14px; color: #999999;'>Best regards,<br/>The TOURNAMATE Team</p>
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
<body style='font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;'>
    <div style='max-width: 600px; margin: auto; background-color: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0,0,0,0.1);'>

        <div style='text-align: center;'>
            <img src='https://cdn-icons-png.flaticon.com/512/1055/1055646.png' alt='Champion Cup' style='width: 80px; height: auto; margin-bottom: 20px;'/>
            <h1 style='color: #2c3e50;'>TOURNAMATE</h1>
        </div>

        <p style='font-size: 16px; color: #333333;'>Hello <strong>{name}</strong>,</p>
        <p style='font-size: 16px; color: #333333;'>
            You requested for a forgotpassword link. Please verify your password link by clicking the button below:
        </p>

        <div style='text-align: center; margin: 30px 0;'>
            <a href='{callbackUrl}' style='display: inline-block; padding: 12px 24px; font-size: 16px; color: #ffffff; background-color: #27ae60; text-decoration: none; border-radius: 5px;'>
                Confirm Your Email
            </a>
        </div>

        <p style='font-size: 14px; color: #666666;'>If the button above doesn't work, copy and paste this URL into your browser:</p>
        <p style='font-size: 14px; color: #666666; word-break: break-all;'>{callbackUrl}</p>

        <hr style='margin: 30px 0; border: none; border-top: 1px solid #eeeeee;'/>
        <p style='font-size: 14px; color: #999999;'>Best regards,<br/>The TOURNAMATE Team</p>
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
