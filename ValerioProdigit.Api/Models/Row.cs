using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ValerioProdigit.Api.Models;

public sealed class Row
{
	[Key]
	public int Id { get; set; }
	public int Index { get; set; }
	public int Seats { get; set; }

	[ForeignKey(nameof(Classroom))]
	public int ClassroomId { get; set; }

	public Classroom Classroom { get; set; } = default!;
}