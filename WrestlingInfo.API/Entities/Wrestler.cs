using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WrestlingInfo.API.Entities;

public class Wrestler {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	
	[MaxLength(200)]
	public string Name { get; set; }
	
	public Wrestler(string name) {
		Name = name;
	}
}