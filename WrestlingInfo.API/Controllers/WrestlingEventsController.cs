using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WrestlingInfo.API.Models;
using WrestlingInfo.API.Services;

namespace WrestlingInfo.API.Controllers;

[ApiController]
[Route("api/promotions/{promotionId}/events")]
public class WrestlingEventsController : ControllerBase {
	private readonly ILogger<PromotionsController> _logger;
	private readonly IWrestlingInfoRepository _wrestlingInfoRepository;
	private readonly IMapper _mapper;

	public WrestlingEventsController(ILogger<PromotionsController> logger, IWrestlingInfoRepository wrestlingInfoRepository, IMapper mapper) {
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_wrestlingInfoRepository = wrestlingInfoRepository ?? throw new ArgumentNullException(nameof(wrestlingInfoRepository));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
	}
	
	[HttpGet]
	public async Task<ActionResult<IEnumerable<WrestlingEventWithoutReviewsDto>>> GetEvents(int promotionId) {
		if (!await _wrestlingInfoRepository.PromotionExistsAsync(promotionId)) {
			_logger.LogInformation($"Promotion with id {promotionId} wasn't found when accessing wrestling events.");
			return NotFound();
		}
		
		var wrestlingEventForPromotion = await _wrestlingInfoRepository.GetWrestlingEventsForPromotionAsync(promotionId);
		return Ok(_mapper.Map<IEnumerable<WrestlingEventWithoutReviewsDto>>(wrestlingEventForPromotion));
	}
	
	[HttpGet("{eventId}", Name = "GetEvent")]
	public async Task<IActionResult> GetEvent(int promotionId, int eventId, bool includeReviews = false) {
		if (!await _wrestlingInfoRepository.PromotionExistsAsync(promotionId)) {
			return NotFound();
		}
		
		var wrestlingEvent = await _wrestlingInfoRepository.GetWrestlingEventForPromotionAsync(promotionId, eventId, includeReviews);

		if (wrestlingEvent is null) {
			return NotFound();
		}

		if (includeReviews) {
			return Ok(_mapper.Map<WrestlingEventDto>(wrestlingEvent));
		}

		return Ok(_mapper.Map<WrestlingEventWithoutReviewsDto>(wrestlingEvent));
	}
}