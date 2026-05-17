using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using blazor_project.Models.Domain;

namespace blazor_project.Services.Auth;

public class SmtpOptions
{
    public string Host { get; set; } = "smtp.gmail.com";
    public int Port { get; set; } = 587;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FromAddress { get; set; } = string.Empty;
    public string FromName { get; set; } = "Customer Manager";
    public bool EnableSsl { get; set; } = true;
}

public class GmailEmailSender : IEmailSender<ApplicationUser>
{
    private readonly SmtpOptions _smtp;
    private readonly ILogger<GmailEmailSender> _logger;

    public GmailEmailSender(IOptions<SmtpOptions> smtpOptions, ILogger<GmailEmailSender> logger)
    {
        _smtp = smtpOptions.Value;
        _logger = logger;
    }

    public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        => SendAsync(email, "Confirm your email",
            $"<p>Hi {WebUtility.HtmlEncode(user.FirstName)},</p>" +
            $"<p>Please confirm your account by <a href=\"{confirmationLink}\">clicking here</a>.</p>");

    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        => SendAsync(email, "Reset your password",
            $"<p>Reset your password by <a href=\"{resetLink}\">clicking here</a>.</p>");

    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        => SendAsync(email, "Reset your password",
            $"<p>Your password reset code is: <strong>{resetCode}</strong></p>");

    private async Task SendAsync(string to, string subject, string htmlBody)
    {
        if (string.IsNullOrWhiteSpace(_smtp.Username) || string.IsNullOrWhiteSpace(_smtp.Password))
        {
            _logger.LogWarning(
                "SMTP credentials not configured. Email to {To} not sent. Subject: {Subject}. Body: {Body}",
                to, subject, htmlBody);
            return;
        }

        using var message = new MailMessage
        {
            From = new MailAddress(
                string.IsNullOrWhiteSpace(_smtp.FromAddress) ? _smtp.Username : _smtp.FromAddress,
                _smtp.FromName),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        message.To.Add(to);

        using var client = new SmtpClient(_smtp.Host, _smtp.Port)
        {
            EnableSsl = _smtp.EnableSsl,
            Credentials = new NetworkCredential(_smtp.Username, _smtp.Password)
        };

        try
        {
            await client.SendMailAsync(message);
            _logger.LogInformation("Sent email to {To} (subject: {Subject})", to, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}", to);
            throw;
        }
    }
}
