namespace ValerioProdigit.Api.Dtos.Classroom;

public class AddClassroomResponse
{
    public bool Success => Error.Length == 0;
    public string Error { get; set; } = "";
}