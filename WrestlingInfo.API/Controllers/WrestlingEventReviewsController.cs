using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WrestlingInfo.API.Entities;
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
		if (!await _wrestlingInfoRepository.WrestlingEventExistsForPromotionAsync(promotionId, eventId)) {
			_logger.LogInformation(
				$"Wrestling Event with id {eventId} wasn't found when accessing wrestling event reviews."
			);
			return NotFound();
		}

		var reviews = await _wrestlingInfoRepository.GetReviewsForWrestlingEventAsync(eventId);
		return Ok(_mapper.Map<IEnumerable<WrestlingEventReviewDto>>(reviews));
	}

	[HttpGet("{reviewId}", Name = "GetWrestlingEventReview")]
	public async Task<ActionResult<WrestlingEventReviewDto>> GetWrestlingEventReview(int promotionId, int eventId, int reviewId) {
		if (!await _wrestlingInfoRepository.WrestlingEventExistsForPromotionAsync(promotionId, eventId)) {
			return NotFound();
		}

		var review = await _wrestlingInfoRepository.GetReviewForWrestlingEventAsync(eventId, reviewId);

		if (review is null) {
			return NotFound();
		}

		return Ok(_mapper.Map<WrestlingEventReviewDto>(review));
	}

	[HttpPost]
	public async Task<ActionResult<WrestlingEventReviewDto>> CreateWrestlingEventReview(
		int promotionId, int eventId, WrestlingEventReviewForCreationDto review
	) {
		if (!await _wrestlingInfoRepository.WrestlingEventExistsForPromotionAsync(promotionId, eventId)) {
			return NotFound();
		}

		var finalReview = _mapper.Map<WrestlingEventReview>(review);

		await _wrestlingInfoRepository.AddReviewForWrestlingEvent(eventId, finalReview);

		await _wrestlingInfoRepository.SaveChangesAsync();

		var createdReview = _mapper.Map<WrestlingEventReviewDto>(finalReview);

		return CreatedAtRoute(
			"GetWrestlingEventReview", new {
				promotionId,
				eventId,
				reviewId = createdReview.Id
			}, createdReview
		);
	}

	[HttpPut("{reviewId}")]
	public async Task<ActionResult> UpdateWrestlingEventReview(
		int promotionId, int eventId, int reviewId, WrestlingEventReviewForUpdateDto wrestlingEventReview
	) {
		if (!await _wrestlingInfoRepository.WrestlingEventExistsForPromotionAsync(promotionId, eventId)) {
			return NotFound();
		}

		var reviewEntity = await _wrestlingInfoRepository.GetReviewForWrestlingEventAsync(eventId, reviewId);

		if (reviewEntity is null) {
			return NotFound();
		}

		_mapper.Map(wrestlingEventReview, reviewEntity);

		await _wrestlingInfoRepository.SaveChangesAsync();

		// Return nothing because the consumer provided the updated dto
		return NoContent();
	}

	[HttpPatch("{reviewId}")]
	public async Task<ActionResult> PartiallyUpdateWrestlingEventReview(
		int promotionId, int eventId, int reviewId, JsonPatchDocument<WrestlingEventReviewForUpdateDto> patchDocument
	) {
		if (!await _wrestlingInfoRepository.WrestlingEventExistsForPromotionAsync(promotionId, eventId)) {
			return NotFound();
		}

		var reviewEntity = await _wrestlingInfoRepository.GetReviewForWrestlingEventAsync(eventId, reviewId);

		if (reviewEntity is null) {
			return NotFound();
		}

		var wrestlingEventReviewToPatch = _mapper.Map<WrestlingEventReviewForUpdateDto>(reviewEntity);

		patchDocument.ApplyTo(wrestlingEventReviewToPatch, ModelState);

		if (!ModelState.IsValid) {
			return BadRequest(ModelState);
		}

		if (!TryValidateModel(wrestlingEventReviewToPatch)) {
			return BadRequest(ModelState);
		}

		_mapper.Map(wrestlingEventReviewToPatch, reviewEntity);

		await _wrestlingInfoRepository.SaveChangesAsync();

		return NoContent();
	}

	[HttpDelete("{reviewId}")]
	public async Task<ActionResult> DeleteWrestlingEventReview(int promotionId, int eventId, int reviewId) {
		if (!await _wrestlingInfoRepository.WrestlingEventExistsForPromotionAsync(promotionId, eventId)) {
			return NotFound();
		}

		var reviewEntity = await _wrestlingInfoRepository.GetReviewForWrestlingEventAsync(eventId, reviewId);

		if (reviewEntity is null) {
			return NotFound();
		}

		_wrestlingInfoRepository.DeleteReview(reviewEntity);
		await _wrestlingInfoRepository.SaveChangesAsync();

		_mailService.Send("WrestlingEventReview deleted.", $"WrestlingEventReview with id {reviewEntity.Id} was deleted.");

		return NoContent();
	}
}