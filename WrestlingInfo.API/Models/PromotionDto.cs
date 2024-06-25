namespace WrestlingInfo.API.Models;

public class PromotionDto {
	public int Id { get; set; }
	public string Name { get; set; }
	public ICollection<WrestlingEventDto> Events { get; set; } = new List<WrestlingEventDto>();
}