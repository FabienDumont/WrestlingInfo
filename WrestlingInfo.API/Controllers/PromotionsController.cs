using Microsoft.AspNetCore.Mvc;
using WrestlingInfo.API.Models;

namespace WrestlingInfo.API.Controllers;

[ApiController]
[Route("api/promotions")]
public class PromotionsController : ControllerBase {
	private readonly ILogger<PromotionsController> _logger;
	private readonly WrestlingDataStore _wrestlingDataStore;

	public PromotionsController(ILogger<PromotionsController> logger, WrestlingDataStore wrestlingDataStore) {
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_wrestlingDataStore = wrestlingDataStore ?? throw new ArgumentNullException(nameof(wrestlingDataStore));
	}

	[HttpGet]
	public ActionResult<IEnumerable<PromotionDto>> GetPromotions() {
		return Ok(_wrestlingDataStore.Promotions);
	}

	[HttpGet("{id}")]
	public ActionResult<PromotionDto> GetPromotion(int id) {
		try {
			PromotionDto? promotionToReturn = _wrestlingDataStore.Promotions.FirstOrDefault(p => p.Id == id);

			if (promotionToReturn is null) {
				return NotFound();
			}

			return Ok(promotionToReturn);
		} catch (Exception ex) {
			_logger.LogCritical($"Exception while getting promotion with id {id}.", ex);
			return StatusCode(500, "A problem happened while handling your request.");
		}
	}
}