using ValerioProdigit.Api.Models;

namespace ValerioProdigit.Api.Emails;

public class EmptyEmailService : IEmailSender
{
	public Task<bool> SendRegisterConfirmation(ApplicationUser user, string link)
	{
		return Task.FromResult(true);
	}

	public Task<bool> SendReservationCreated(ApplicationUser user, Reservation reservation)
	{
		return Task.FromResult(true);
	}
}