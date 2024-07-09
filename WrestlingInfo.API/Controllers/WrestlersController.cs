using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WrestlingInfo.API.Entities;
using WrestlingInfo.API.Models;
using WrestlingInfo.API.Services;

namespace WrestlingInfo.API.Controllers;

[ApiController]
[Route("api/wrestlers")]
public class WrestlersController : ControllerBase {
	private readonly IWrestlingInfoRepository _wrestlingInfoRepository;
	private readonly IMapper _mapper;

	public WrestlersController(IWrestlingInfoRepository wrestlingInfoRepository, IMapper mapper) {
		_wrestlingInfoRepository = wrestlingInfoRepository ?? throw new ArgumentNullException(nameof(wrestlingInfoRepository));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
	}
	
	[HttpGet]
	public async Task<ActionResult<IEnumerable<WrestlerDto>>> GetWrestlers() {
		IEnumerable<Wrestler>? wrestlerEntities = await _wrestlingInfoRepository.GetWrestlersAsync();
		return Ok(_mapper.Map<IEnumerable<WrestlerDto>>(wrestlerEntities));
	}
	
	[HttpGet("{id}")]
	public async Task<ActionResult<PromotionDto>> GetWrestler(int id) {
		Wrestler? wrestler = await _wrestlingInfoRepository.GetWrestlerAsync(id);

		if (wrestler is null) {
			return NotFound();
		}

		return Ok(_mapper.Map<WrestlerDto>(wrestler));
	}
}