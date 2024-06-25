using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WrestlingInfo.API.Entities;

public class WrestlingEvent {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	
	[MaxLength(200)]
	public string Name { get; set; }
	public DateOnly Date { get; set; }
	public ICollection<WrestlingEventReview> Reviews { get; set; } = new List<WrestlingEventReview>();
	
	[ForeignKey("PromotionId")]
	public Promotion? Promotion { get; set; }
	public int PromotionId { get; set; }
	
	public WrestlingEvent(string name) {
		Name = name;
	}
}