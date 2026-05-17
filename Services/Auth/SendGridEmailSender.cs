using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using blazor_project.Models.Domain;

namespace blazor_project.Services.Auth;

public class SendGridOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string FromAddress { get; set; } = string.Empty;
    public string FromName { get; set; } = "Customer Manager";
}

/// <summary>
/// Sends Identity emails via SendGrid's HTTPS API. Works on hosts (like Render's
/// free tier) that block outbound SMTP ports.
/// </summary>
public class SendGridEmailSender : IEmailSender<ApplicationUser>
{
    private readonly SendGridOptions _options;
    private readonly ILogger<SendGridEmailSender> _logger;

    public SendGridEmailSender(IOptions<SendGridOptions> options, ILogger<SendGridEmailSender> logger)
    {
        _options = options.Value;
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
        if (string.IsNullOrWhiteSpace(_options.ApiKey) || string.IsNullOrWhiteSpace(_options.FromAddress))
        {
            _logger.LogWarning(
                "SendGrid not configured (missing ApiKey or FromAddress). Email to {To} not sent. Subject: {Subject}",
                to, subject);
            return;
        }

        var client = new SendGridClient(_options.ApiKey);
        var msg = MailHelper.CreateSingleEmail(
            new EmailAddress(_options.FromAddress, _options.FromName),
            new EmailAddress(to),
            subject,
            plainTextContent: null,
            htmlContent: htmlBody);

        var response = await client.SendEmailAsync(msg);
        if ((int)response.StatusCode >= 400)
        {
            var body = await response.Body.ReadAsStringAsync();
            _logger.LogError(
                "SendGrid returned {Status} for email to {To}. Body: {Body}",
                response.StatusCode, to, body);
            throw new InvalidOperationException(
                $"SendGrid send failed ({response.StatusCode}): {body}");
        }

        _logger.LogInformation("Sent email to {To} via SendGrid (subject: {Subject})", to, subject);
    }
}
