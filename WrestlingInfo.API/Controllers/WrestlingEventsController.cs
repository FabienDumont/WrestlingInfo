using Microsoft.AspNetCore.Mvc;
using WrestlingInfo.API.Models;

namespace WrestlingInfo.API.Controllers;

[ApiController]
[Route("api/promotions/{promotionId}/events")]
public class WrestlingEventsController : ControllerBase {
	private readonly WrestlingDataStore _wrestlingDataStore;

	public WrestlingEventsController(WrestlingDataStore wrestlingDataStore) {
		_wrestlingDataStore = wrestlingDataStore ?? throw new ArgumentNullException(nameof(wrestlingDataStore));
	}
	
	[HttpGet]
	public ActionResult<IEnumerable<WrestlingEventDto>> GetEvents(int promotionId) {
		PromotionDto? promotion = _wrestlingDataStore.Promotions.FirstOrDefault(p => p.Id == promotionId);
		if (promotion is null) {
			return NotFound();
		}

		return Ok(promotion.Events);
	}
	
	[HttpGet("{eventId}", Name = "GetEvent")]
	public ActionResult<WrestlingEventDto> GetEvent(int promotionId, int wrestlingEventId) {
		PromotionDto? promotion = _wrestlingDataStore.Promotions.FirstOrDefault(p => p.Id == promotionId);
		if (promotion is null) {
			return NotFound();
		}

		WrestlingEventDto? wrestlingEventToReturn = promotion.Events.FirstOrDefault(e => e.Id == wrestlingEventId);

		if (wrestlingEventToReturn is null) {
			return NotFound();
		}

		return Ok(wrestlingEventToReturn);
	}
}