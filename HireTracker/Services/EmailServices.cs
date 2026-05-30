using Microsoft.AspNetCore.Identity.UI.Services;
using Resend;

namespace HireTracker.Services
{
    public class EmailServices:IEmailService
    {
        public readonly IResend _resend;
        private readonly IConfiguration _config;

        public EmailServices(IConfiguration config, IResend resend)
        {
            _resend = resend;
            _config = config;
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
        {
            var message = new EmailMessage();
            message.From = "onboarding@resend.dev";
            message.To.Add(toEmail);
            message.Subject = "Reset your HireTracker password";
            message.HtmlBody = $@"
                <h3>Password Reset Request</h3>
                <p>Click the link below to reset your password:</p>
                <a href='{resetLink}'>Reset Password</a>
                <p>If you did not request a password reset, please ignore this email.</p>
            ";

            await _resend.EmailSendAsync(message);
        }
    }
}
