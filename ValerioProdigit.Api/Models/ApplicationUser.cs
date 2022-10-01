using Microsoft.AspNetCore.Identity;

namespace ValerioProdigit.Api.Models;

public sealed class ApplicationUser : IdentityUser<int>
{
	public string Surname { get; set; } = null!;
	public string Name { get; set; } = null!;

	public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}