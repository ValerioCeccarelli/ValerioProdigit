namespace ValerioProdigit.Api.Dtos.Classroom;

public class GetAllByBuildingResponse
{
    public bool Success => Error.Length == 0;
    public string Error { get; set; } = "";

    public IEnumerable<Classroom> Classrooms { get; set; } = Enumerable.Empty<Classroom>();

    public class Classroom
    {
        public string Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public int Capacity { get; set; }
        public IEnumerable<int> Rows { get; set; } = default!;
    }
}