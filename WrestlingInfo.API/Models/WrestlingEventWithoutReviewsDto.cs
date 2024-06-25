namespace WrestlingInfo.API.Models;

public class WrestlingEventWithoutReviewsDto {
	public int Id { get; set; }
	public string Name { get; set; }
	public DateOnly Date { get; set; }
}