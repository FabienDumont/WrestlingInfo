using WrestlingInfo.API.Models;

namespace WrestlingInfo.API;

public class WrestlingDataStore {
	public List<PromotionDto> Promotions { get; set; }
	public List<WrestlerDto> Wrestlers { get; set; }

	public WrestlingDataStore() {
		Promotions = new List<PromotionDto> {
			new() {
				Id = 1,
				Name = "WWE",
				Events = new List<WrestlingEventDto> {
					new() {
						Id = 1,
						Name = "Raw",
						Date = new DateOnly(2024, 1, 1)
					},
					new() {
						Id = 2,
						Name = "Smackdown",
						Date = new DateOnly(2024, 1, 5)
					}
				}
			}
		};

		Wrestlers = new List<WrestlerDto> {
			new() {
				Id = 1,
				Name = "AJ Styles"
			},
			new() {
				Id = 2,
				Name = "Akira Tozawa"
			},
			new() {
				Id = 3,
				Name = "Akam"
			},
			new() {
				Id = 4,
				Name = "Alba Fyre"
			},
			new() {
				Id = 5,
				Name = "Andrade"
			}
		};
	}
}