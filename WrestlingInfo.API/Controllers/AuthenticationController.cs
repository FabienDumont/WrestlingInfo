using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WrestlingInfo.API.Entities;
using WrestlingInfo.API.Models;
using WrestlingInfo.API.Profiles;
using WrestlingInfo.API.Services;

namespace WrestlingInfo.API.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase {
	private readonly IWrestlingInfoRepository _wrestlingInfoRepository;
	private readonly IMapper _mapper;
	private readonly IConfiguration _configuration;

	public class AuthenticationRequestBody {
		public string? UserName { get; set; }
		public string? Password { get; set; }
	}

	public AuthenticationController(IWrestlingInfoRepository wrestlingInfoRepository, IMapper mapper, IConfiguration configuration) {
		_wrestlingInfoRepository = wrestlingInfoRepository ?? throw new ArgumentNullException(nameof(wrestlingInfoRepository));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
	}

	[HttpPost("authenticate")]
	public ActionResult<string> Authenticate(AuthenticationRequestBody authenticationRequestBody) {
		var user = ValidateUserCredentials(authenticationRequestBody.UserName, authenticationRequestBody.Password);
		
		return Ok("TODO");
	}

	private async Task<IActionResult> ValidateUserCredentials(string? userName, string? password) {
		var user = await _wrestlingInfoRepository.GetUserAsync(userName, password);

		return Ok(_mapper.Map<UserDto>(user));
	}
}