namespace ValerioProdigit.Api.Dtos.Reservation;

public sealed class GetByLessonResponse
{
	public bool Success => Error.Length == 0;
	public string Error { get; set; } = "";

	public IEnumerable<ReservationDto> Reservations { get; set; } = Enumerable.Empty<ReservationDto>();
	
	public class ReservationDto
	{
		public string Name { get; set; } = default!;
		public string Surname { get; set; } = default!;
		public string Email { get; set; } = default!;

		public string ReservationId { get; set; } = default!;
	}
}