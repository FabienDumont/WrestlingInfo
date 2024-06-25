namespace WrestlingInfo.API.Models;

public class WrestlingEventDto {
	public int Id { get; set; }
	public string Name { get; set; }
	public DateOnly Date { get; set; }
	public ICollection<WrestlingEventReviewDto> Reviews { get; set; } = new List<WrestlingEventReviewDto>();
}