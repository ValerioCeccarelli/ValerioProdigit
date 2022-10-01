namespace ValerioProdigit.Api.Dtos.Reservation;

public sealed class DeleteReservationResponse
{
	public bool Success => Error.Length == 0;
	public string Error { get; set; } = "";
}