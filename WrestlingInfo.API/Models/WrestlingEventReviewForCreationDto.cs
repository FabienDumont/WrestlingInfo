using System.ComponentModel.DataAnnotations;
using WrestlingInfo.API.Models.Validation;

namespace WrestlingInfo.API.Models;

public class WrestlingEventReviewForCreationDto {
	[Required]
	[DoubleRange(AllowableValues = [0, 0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2, 2.25, 2.5, 2.75, 3, 3.25, 3.5, 3.75, 4, 4.25, 4.5, 4.75, 5])]
	public double Rating { get; set; }
	[MaxLength(200)]
	public string? Comment { get; set; }
}