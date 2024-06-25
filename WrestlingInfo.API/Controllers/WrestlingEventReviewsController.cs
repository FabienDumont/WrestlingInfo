using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WrestlingInfo.API.Models;
using WrestlingInfo.API.Services;

namespace WrestlingInfo.API.Controllers;

[ApiController]
[Route("api/promotions/{promotionId}/events/{eventId}/reviews")]
public class WrestlingEventReviewsController : ControllerBase {
	private readonly ILogger<PromotionsController> _logger;
	private readonly IMailService _mailService;
	private readonly IWrestlingInfoRepository _wrestlingInfoRepository;
	private readonly IMapper _mapper;

	public WrestlingEventReviewsController(
		ILogger<PromotionsController> logger, IMailService mailService, IWrestlingInfoRepository wrestlingInfoRepository, IMapper mapper
	) {
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
		_wrestlingInfoRepository = wrestlingInfoRepository ?? throw new ArgumentNullException(nameof(wrestlingInfoRepository));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<WrestlingEventReviewDto>>> GetWrestlingEventReviews(int promotionId, int eventId) {
		if (!await _wrestlingInfoRepository.WrestlingEventExistsAsync(promotionId, eventId)) {
			_logger.LogInformation(
				$"Wrestling Event with id {eventId} from Promotion with id {promotionId} wasn't found when accessing wrestling event reviews."
			);
			return NotFound();
		}

		var reviews = await _wrestlingInfoRepository.GetReviewsForWrestlingEventAsync(promotionId, eventId);
		return Ok(_mapper.Map<IEnumerable<WrestlingEventReviewDto>>(reviews));
	}

	[HttpGet("{reviewId}", Name = "GetWrestlingEventReview")]
	public async Task<ActionResult<WrestlingEventReviewDto>> GetWrestlingEventReview(int promotionId, int eventId, int reviewId) {
		if (!await _wrestlingInfoRepository.WrestlingEventExistsAsync(promotionId, eventId)) {
			return NotFound();
		}

		var review = await _wrestlingInfoRepository.GetReviewForWrestlingEventAsync(promotionId, eventId, reviewId);

		if (review is null) {
			return NotFound();
		}

		return Ok(_mapper.Map<WrestlingEventReviewDto>(review));
	}

	/*[HttpPost]
	public ActionResult<WrestlingEventReviewDto> CreateWrestlingEventReview(int promotionId, int wrestlingEventId, WrestlingEventReviewForCreationDto review) {
		PromotionDto? promotion = _wrestlingDataStore.Promotions.FirstOrDefault(p => p.Id == promotionId);
		if (promotion is null) {
			return NotFound();
		}

		WrestlingEventDto? wrestlingEvent = promotion.Events.FirstOrDefault(e => e.Id == wrestlingEventId);
		if (wrestlingEvent is null) {
			return NotFound();
		}

		int maxReviewId = 0;
		if (wrestlingEvent.Reviews.Count > 0) {
			maxReviewId = wrestlingEvent.Reviews.Max(r => r.Id);
		}

		WrestlingEventReviewDto finalWrestlingEventReview = new() {
			Id = ++maxReviewId,
			Rating = review.Rating,
			Comment = review.Comment
		};

		wrestlingEvent.Reviews.Add(finalWrestlingEventReview);

		return CreatedAtRoute(
			"GetWrestlingEventReview", new {
				promotionId,
				wrestlingEventId,
				reviewId = finalWrestlingEventReview.Id
			}, finalWrestlingEventReview
		);
	}

	[HttpPut("{reviewId}")]
	public ActionResult UpdateWrestlingEventReview(int promotionId, int wrestlingEventId, int reviewId, WrestlingEventReviewForUpdateDto wrestlingEventReview) {
		PromotionDto? promotion = _wrestlingDataStore.Promotions.FirstOrDefault(p => p.Id == promotionId);
		if (promotion is null) {
			return NotFound();
		}

		WrestlingEventDto? wrestlingEvent = promotion.Events.FirstOrDefault(e => e.Id == wrestlingEventId);
		if (wrestlingEvent is null) {
			return NotFound();
		}

		WrestlingEventReviewDto? reviewFromStore = wrestlingEvent.Reviews.FirstOrDefault(r => r.Id == reviewId);
		if (reviewFromStore is null) {
			return NotFound();
		}

		reviewFromStore.Rating = wrestlingEventReview.Rating;
		reviewFromStore.Comment = wrestlingEventReview.Comment;

		// Return nothing because the consumer provided the updated dto
		return NoContent();
	}

	[HttpPatch("{reviewId}")]
	public ActionResult PartiallyUpdateWrestlingEventReview(
		int promotionId, int wrestlingEventId, int reviewId, JsonPatchDocument<WrestlingEventReviewForUpdateDto> patchDocument
	) {
		PromotionDto? promotion = _wrestlingDataStore.Promotions.FirstOrDefault(p => p.Id == promotionId);
		if (promotion is null) {
			return NotFound();
		}

		WrestlingEventDto? wrestlingEvent = promotion.Events.FirstOrDefault(e => e.Id == wrestlingEventId);
		if (wrestlingEvent is null) {
			return NotFound();
		}

		WrestlingEventReviewDto? reviewFromStore = wrestlingEvent.Reviews.FirstOrDefault(r => r.Id == reviewId);
		if (reviewFromStore is null) {
			return NotFound();
		}

		WrestlingEventReviewForUpdateDto wrestlingEventReviewToPatch = new() {
			Rating = reviewFromStore.Rating,
			Comment = reviewFromStore.Comment
		};

		patchDocument.ApplyTo(wrestlingEventReviewToPatch, ModelState);

		if (!ModelState.IsValid) {
			return BadRequest(ModelState);
		}

		if (!TryValidateModel(wrestlingEventReviewToPatch)) {
			return BadRequest(ModelState);
		}

		reviewFromStore.Rating = wrestlingEventReviewToPatch.Rating;
		reviewFromStore.Comment = wrestlingEventReviewToPatch.Comment;

		return NoContent();
	}

	[HttpDelete("{reviewId}")]
	public ActionResult DeleteWrestlingEventReview(int promotionId, int wrestlingEventId, int reviewId) {
		PromotionDto? promotion = _wrestlingDataStore.Promotions.FirstOrDefault(p => p.Id == promotionId);
		if (promotion is null) {
			return NotFound();
		}

		WrestlingEventDto? wrestlingEvent = promotion.Events.FirstOrDefault(e => e.Id == wrestlingEventId);
		if (wrestlingEvent is null) {
			return NotFound();
		}

		WrestlingEventReviewDto? reviewFromStore = wrestlingEvent.Reviews.FirstOrDefault(r => r.Id == reviewId);
		if (reviewFromStore is null) {
			return NotFound();
		}

		wrestlingEvent.Reviews.Remove(reviewFromStore);

		_mailService.Send(
			"WrestlingEventReview deleted.", $"WrestlingEventReview with id {reviewFromStore.Id} of event {wrestlingEvent.Name} ({promotion.Name}) was deleted."
		);

		return NoContent();
	}*/
}