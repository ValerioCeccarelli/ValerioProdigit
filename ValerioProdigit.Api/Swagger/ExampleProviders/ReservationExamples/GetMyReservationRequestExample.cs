using Swashbuckle.AspNetCore.Filters;
using ValerioProdigit.Api.Dtos.Reservation;

namespace ValerioProdigit.Api.Swagger.ExampleProviders.ReservationExamples;

public class GetMyReservationRequestExample : IExamplesProvider<GetMyReservationRequest>
{
	public GetMyReservationRequest GetExamples()
	{
		return new GetMyReservationRequest()
		{
			Date = DateTime.UtcNow.ToString("yyyy-MM-dd")
		};
	}
}