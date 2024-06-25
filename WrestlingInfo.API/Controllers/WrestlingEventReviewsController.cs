using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WrestlingInfo.API.Models;
using WrestlingInfo.API.Services;

namespace WrestlingInfo.API.Controllers;

[ApiController]
[Route("api/promotions/{promotionId}/events/{wrestlingEventId}/reviews")]
public class WrestlingEventReviewsController : ControllerBase {
	private readonly IMailService _mailService;
	private readonly WrestlingDataStore _wrestlingDataStore;

	public WrestlingEventReviewsController(IMailService mailService, WrestlingDataStore wrestlingDataStore) {
		_mailService = mailService;
		_wrestlingDataStore = wrestlingDataStore ?? throw new ArgumentNullException(nameof(wrestlingDataStore));
	}
	
	[HttpGet]
	public ActionResult<IEnumerable<WrestlingEventReviewDto>> GetWrestlingEventReviews(int promotionId, int wrestlingEventId) {
		PromotionDto? promotion = _wrestlingDataStore.Promotions.FirstOrDefault(p => p.Id == promotionId);
		if (promotion is null) {
			return NotFound();
		}

		WrestlingEventDto? wrestlingEventDto = promotion.Events.FirstOrDefault(e => e.Id == wrestlingEventId);
		if (wrestlingEventDto is null) {
			return NotFound();
		}

		return Ok(wrestlingEventDto.Reviews);
	}

	[HttpGet("{reviewId}", Name = "GetWrestlingEventReview")]
	public ActionResult<WrestlingEventReviewDto> GetWrestlingEventReview(int promotionId, int wrestlingEventId, int reviewId) {
		PromotionDto? promotion = _wrestlingDataStore.Promotions.FirstOrDefault(p => p.Id == promotionId);
		if (promotion is null) {
			return NotFound();
		}

		WrestlingEventDto? wrestlingEventDto = promotion.Events.FirstOrDefault(e => e.Id == wrestlingEventId);
		if (wrestlingEventDto is null) {
			return NotFound();
		}

		WrestlingEventReviewDto? reviewToReturn = wrestlingEventDto.Reviews.FirstOrDefault(r => r.Id == reviewId);

		if (reviewToReturn is null) {
			return NotFound();
		}

		return Ok(reviewToReturn);
	}

	[HttpPost]
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
	public ActionResult PartiallyUpdateWrestlingEventReview(int promotionId, int wrestlingEventId, int reviewId, JsonPatchDocument<WrestlingEventReviewForUpdateDto> patchDocument) {
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
		
		_mailService.Send("WrestlingEventReview deleted.", $"WrestlingEventReview with id {reviewFromStore.Id} of event {wrestlingEvent.Name} ({promotion.Name}) was deleted.");
		
		return NoContent();
	}
}