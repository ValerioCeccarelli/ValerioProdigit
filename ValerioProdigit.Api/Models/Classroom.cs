using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ValerioProdigit.Api.Models;

public class Classroom
{
	[Key]
	public int Id { get; set; }
	
	public int Capacity { get; set; }

	public ICollection<Row> Rows { get; set; } = new List<Row>();

	public string Code { get; set; } = null!;

	[ForeignKey(nameof(Building))]
	public int BuildingId { get; set; }
	public Building Building { get; set; } = null!;
}