namespace ValerioProdigit.Api.Dtos.Reservation;

public sealed class GetReservationResponse
{
	public bool Success => Error.Length == 0;
	public string Error { get; set; } = "";

	public string Name { get; set; } = default!;
	public string Surname { get; set; } = default!;
	public string Email { get; set; } = default!;
	
	public int Row { get; set; }
	public int Seat { get; set; }
	
	public string LessonName { get; set; } = default!;

	public string LessonId { get; set; } = default!;
}