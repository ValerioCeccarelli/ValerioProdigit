using ValerioProdigit.Api.Models;

namespace ValerioProdigit.Api.Emails;

public interface IEmailSender
{
    Task<bool> SendRegisterConfirmation(ApplicationUser user, string link);
    Task<bool> SendReservationCreated(ApplicationUser user, Reservation reservation);
}