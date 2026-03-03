
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AuthService.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task SendPasswordResetEmailAsync(string email, string resetLink)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                _configuration["Email:DisplayName"] ?? "CamEatWell",
                _configuration["Email:Address"]
                ));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Reset your password";

            var bodyBuilder = new BodyBuilder();

            bodyBuilder.HtmlBody = $@"  
                 <html>
                <body>
                    <h2>Password Reset Request</h2>
                    <p>You requested to reset your password. Click the button below to proceed:</p>
                    <a href='{resetLink}' style='
                        display: inline-block; 
                        padding: 10px 20px; 
                        background-color: #007bff;
                        color: white;
                        text-decoration: none;
                        border-radius: 5px;
                        margin: 10px 0;
                    '>Reset Password</a>
                    <p>Or copy and paste this link in your browser:</p>
                    <p style='background-color: #f8f9fa; padding: 10px; border-radius: 5px;'>
                        {resetLink}
                    </p>
                    <p>This link will expire in 30 minutes.</p>
                    <p>If you didn't request this, please ignore this email.</p>
                </body>
                </html>";

            bodyBuilder.TextBody = $"Reset your password by clicking this link: {resetLink}\n\n" +
                                   "This link expires in 30 minutes.\n" +
                                   "If you didn't request this, please ignore this email.";

            message.Body = bodyBuilder.ToMessageBody();
            using var smtp = new SmtpClient();

            try
            {
                _logger.LogInformation($"Attempting to send password reset email to {email}");

                await smtp.ConnectAsync(
                    _configuration["Email:SmtpServer"],
                    int.Parse(_configuration["Email:Port"]!));


                _logger.LogInformation($"Connected to SMTP server: {_configuration["Email:SmtpServer"]}");

                await smtp.AuthenticateAsync(
                    _configuration["Email:Username"],
                    _configuration["Email:Password"]
                    );

                _logger.LogInformation("SMTP authentication successful");

                await smtp.SendAsync(message);
                _logger.LogInformation($"Password reset email sent successfully to {email}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {email}");
                throw;
            }
            finally
            {
                if (smtp.IsConnected)
                {
                    await smtp.DisconnectAsync(true);
                }
            }

        }
    }
}
