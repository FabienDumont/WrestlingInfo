using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WrestlingInfo.API.Entities;

public class WrestlingEventReview {
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public double Rating { get; set; }
	
	[MaxLength(1000)]

	public string? Comment { get; set; }
	
	[ForeignKey("WrestlingEventId")]
	public WrestlingEvent? WrestlingEvent { get; set; }
	public int WrestlingEventId { get; set; }
	
	public WrestlingEventReview(double rating) {
		Rating = rating;
	}
}