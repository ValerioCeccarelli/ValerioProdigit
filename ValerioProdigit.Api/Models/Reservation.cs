using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ValerioProdigit.Api.Models;

public class Reservation
{
	[Key]
	public int Id { get; set; }

	public int Row { get; set; }
	public int Seat { get; set; }

	[ForeignKey(nameof(Lesson))]
	public int LessonId { get; set; }
	public Lesson Lesson { get; set; } = default!;
	
	[ForeignKey(nameof(Student))]
	public int StudentId { get; set; }
	public ApplicationUser Student { get; set; } = default!;
}