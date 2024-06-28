using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WrestlingInfo.API.Models;
using WrestlingInfo.API.Services;

namespace WrestlingInfo.API.Controllers;

[ApiController]
[Route("api/promotions")]
public class PromotionsController : ControllerBase {
	private readonly ILogger<PromotionsController> _logger;
	private readonly IWrestlingInfoRepository _wrestlingInfoRepository;
	private readonly IMapper _mapper;
	private const int MaxPromotionsPageSize = 20;

	public PromotionsController(ILogger<PromotionsController> logger, IWrestlingInfoRepository wrestlingInfoRepository, IMapper mapper) {
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_wrestlingInfoRepository = wrestlingInfoRepository ?? throw new ArgumentNullException(nameof(wrestlingInfoRepository));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<PromotionWithoutWrestlingEventsDto>>> GetPromotions(
		string? name, string? searchQuery, int pageNumber = 1, int pageSize = 10
	) {
		if (pageSize > MaxPromotionsPageSize) {
			pageSize = MaxPromotionsPageSize;
		}
		
		var (promotionEntities, paginationMetadata) = await _wrestlingInfoRepository.GetPromotionsAsync(name, searchQuery, pageNumber, pageSize);
		
		Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
		
		return Ok(_mapper.Map<IEnumerable<PromotionWithoutWrestlingEventsDto>>(promotionEntities));
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetPromotion(int id, bool includeWrestlingEvents = false) {
		try {
			var promotion = await _wrestlingInfoRepository.GetPromotionAsync(id, includeWrestlingEvents);

			if (promotion is null) {
				return NotFound();
			}

			if (includeWrestlingEvents) {
				return Ok(_mapper.Map<PromotionDto>(promotion));
			}

			return Ok(_mapper.Map<PromotionWithoutWrestlingEventsDto>(promotion));
		} catch (Exception ex) {
			_logger.LogCritical($"Exception while getting promotion with id {id}.", ex);
			return StatusCode(500, "A problem happened while handling your request.");
		}
	}
}