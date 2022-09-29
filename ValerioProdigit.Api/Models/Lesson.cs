using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ValerioProdigit.Api.Models;

public class Lesson
{
	[Key]
	public int Id { get; set; }

	public string Date { get; set; } = default!;
	public int StartHour { get; set; }
	public int FinishHour { get; set; }
	
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;
	
	[ForeignKey(nameof(Classroom))]
	public int ClassroomId { get; set; }
	public Classroom Classroom { get; set; } = default!;
	
	[ForeignKey(nameof(Teacher))]
	public int TeacherId { get; set; }
	public ApplicationUser Teacher { get; set; } = default!;
	
	public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}