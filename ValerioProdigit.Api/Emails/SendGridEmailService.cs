using SendGrid;
using SendGrid.Helpers.Mail;
using ValerioProdigit.Api.Models;

namespace ValerioProdigit.Api.Emails;

public class SendGridEmailService : IEmail
{
    private readonly SendGridClient _client;
    private readonly EmailAddress _from;

    public SendGridEmailService(string apyKey, string email, string name)
    {
        _client = new SendGridClient(apyKey);
        _from = new EmailAddress(email, name);
    }
    
    //todo: write better email, this is only for test
    
    public async Task<bool> SendRegisterConfirmation(string email, string name, string link)
    {
        var to = new EmailAddress(email, name);
        const string subject = "ValerioProdigit account confirmation";
        var plainTextContent = $"To confirm your account use this link {link}";
        var htmlContent = $"To confirm your account use this <a href=\"{link}\">link</a>";
        var msg = MailHelper.CreateSingleEmail(_from, to, subject, plainTextContent, htmlContent);
        var response = await _client.SendEmailAsync(msg);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> SendReservationCreated(string email, string name, Reservation reservation)
    {
        var to = new EmailAddress(email, name);
        const string subject = "ValerioProdigit new reservation created";
        var plainTextContent = $"New reservation created\nRow {reservation.Row}, Seat {reservation.Seat}";
        var htmlContent = plainTextContent;
        var msg = MailHelper.CreateSingleEmail(_from, to, subject, plainTextContent, htmlContent);
        var response = await _client.SendEmailAsync(msg);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> SendReservationDeleted(string email, string name, Reservation reservation)
    {
        var to = new EmailAddress(email, name);
        const string subject = "ValerioProdigit reservation deleted";
        var plainTextContent = $"reservation deleted\nRow {reservation.Row}, Seat {reservation.Seat}";
        var htmlContent = plainTextContent;
        var msg = MailHelper.CreateSingleEmail(_from, to, subject, plainTextContent, htmlContent);
        var response = await _client.SendEmailAsync(msg);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> SendReservationDeletedByAdmin(string email, string name, Reservation reservation, string reason)
    {
        var to = new EmailAddress(email, name);
        const string subject = "ValerioProdigit reservation deleted";
        var plainTextContent = $"reservation deleted by an admin\nReason {reason}\nRow {reservation.Row}, Seat {reservation.Seat}";
        var htmlContent = plainTextContent;
        var msg = MailHelper.CreateSingleEmail(_from, to, subject, plainTextContent, htmlContent);
        var response = await _client.SendEmailAsync(msg);
        return response.IsSuccessStatusCode;
    }
}