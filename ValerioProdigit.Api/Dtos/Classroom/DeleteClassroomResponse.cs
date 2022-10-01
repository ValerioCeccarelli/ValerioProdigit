namespace ValerioProdigit.Api.Dtos.Classroom;

public sealed class DeleteClassroomResponse
{
    public bool Success => Error.Length == 0;
    public string Error { get; set; } = "";
}