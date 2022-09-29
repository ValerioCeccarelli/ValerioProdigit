using ValerioProdigit.Api.Models;

namespace ValerioProdigit.Api.Emails;

public interface IEmail
{
    Task<bool> SendRegisterConfirmation(string email, string name, string link);
    Task<bool> SendReservationCreated(string email, string name, Reservation reservation);
    Task<bool> SendReservationDeleted(string email, string name, Reservation reservation);
    Task<bool> SendReservationDeletedByAdmin(string email, string name, Reservation reservation, string reason);
}