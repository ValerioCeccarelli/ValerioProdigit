namespace ValerioProdigit.Api.Dtos.Reservation;

public sealed class AddReservationResponse
{
	public bool Success => Error.Length == 0;
	public string Error { get; set; } = "";
}