using System.ComponentModel.DataAnnotations;

namespace ValerioProdigit.Api.Models;

public sealed class Building
{
	[Key]
	public int Id { get; set; }
	
	public string Name { get; set; } = null!;

	public string Code { get; set; } = null!;

	public string Address { get; set; } = null!;

	public ICollection<Classroom> Classrooms { get; set; } = new List<Classroom>();
}