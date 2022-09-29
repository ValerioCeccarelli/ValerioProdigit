namespace ValerioProdigit.Api.Dtos.Reservation;

public class DeleteReservationResponse
{
	public bool Success => Error.Length == 0;
	public string Error { get; set; } = "";
}