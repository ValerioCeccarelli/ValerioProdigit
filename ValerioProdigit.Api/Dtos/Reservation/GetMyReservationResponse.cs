namespace ValerioProdigit.Api.Dtos.Reservation;

public sealed class GetMyReservationResponse
{
	public bool Success => Error.Length == 0;
	public string Error { get; set; } = "";

	public IEnumerable<ReservationDto> Resevations { get; set; } = Enumerable.Empty<ReservationDto>();

	public class ReservationDto
	{
		public string LessonName { get; set; } = default!;
		public string LessonDescription { get; set; } = default!;
		public string TeacherEmail { get; set; } = default!;
		public string BuildingCode { get; set; } = default!;
		public string ClassroomCode { get; set; } = default!;
		public string Address { get; set; } = default!;

		public int Row { get; set; }
		public int Seat { get; set; }
		
		public string Date { get; set; } = default!;
		public int StartHour { get; set; }
		public int FinishHour { get; set; }

		public string ReservationId { get; set; } = default!;
	}
}