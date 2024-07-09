namespace WrestlingInfo.API.Models;

public class WrestlingEventReviewDto {
	public int Id { get; set; }
	public double Rating { get; set; }

	public string? Comment { get; set; }
}