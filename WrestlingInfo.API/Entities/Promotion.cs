using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WrestlingInfo.API.Entities;

public class Promotion {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	[MaxLength(200)] public string Name { get; set; }

	[MaxLength(1000)] public string? Description { get; set; }

	public ICollection<WrestlingEvent> Events { get; set; } = new List<WrestlingEvent>();

	public Promotion(string name) {
		Name = name;
	}
}