using Microsoft.AspNetCore.Mvc;
using WrestlingInfo.API.Models;

namespace WrestlingInfo.API.Controllers;

[ApiController]
[Route("api/wrestlers")]
public class WrestlersController : ControllerBase {
	private readonly WrestlingDataStore _wrestlingDataStore;

	public WrestlersController(WrestlingDataStore wrestlingDataStore) {
		_wrestlingDataStore = wrestlingDataStore ?? throw new ArgumentNullException(nameof(wrestlingDataStore));
	}
	
	[HttpGet]
	public ActionResult<IEnumerable<WrestlerDto>> GetWrestlers() {
		return Ok(_wrestlingDataStore.Wrestlers);
	}
	
	[HttpGet("{id}")]
	public ActionResult<PromotionDto> GetWrestler(int id) {
		WrestlerDto? wrestlerToReturn = _wrestlingDataStore.Wrestlers.FirstOrDefault(w => w.Id == id);

		if (wrestlerToReturn is null) {
			return NotFound();
		}

		return Ok(wrestlerToReturn);
	}
}