using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WrestlingInfo.API.Entities;
using WrestlingInfo.API.Models;
using WrestlingInfo.API.Services;

namespace WrestlingInfo.API.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase {
	private readonly IWrestlingInfoRepository _wrestlingInfoRepository;
	private readonly IMapper _mapper;
	private readonly ITokenService _tokenService;

	public UsersController(IWrestlingInfoRepository wrestlingInfoRepository, IMapper mapper, ITokenService tokenService) {
		_wrestlingInfoRepository = wrestlingInfoRepository ?? throw new ArgumentNullException(nameof(wrestlingInfoRepository));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		_tokenService = tokenService;
	}


	[HttpPost("register")]
	public async Task<ActionResult<UserDto>> Register(UserRegisterDto userRegisterDto) {
		User? existingUser = await _wrestlingInfoRepository.GetUserAsync(userRegisterDto.Username);
		if (existingUser is not null) {
			return BadRequest("Username is taken");
		}

		using HMACSHA512 hmac = new HMACSHA512();

		User? user = _mapper.Map<User>(userRegisterDto);

		user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userRegisterDto.Password));
		user.PasswordSalt = hmac.Key;

		await _wrestlingInfoRepository.AddUser(user);
		await _wrestlingInfoRepository.SaveChangesAsync();

		UserDto? userDto = _mapper.Map<UserDto>(user);

		userDto.Token = _tokenService.CreateToken(user);

		return Ok(userDto);
	}

	[HttpPost("login")]
	public async Task<ActionResult<UserDto>> Login(UserLoginDto userLoginDto) {
		User? user = await _wrestlingInfoRepository.GetUserAsync(userLoginDto.Username);
		if (user is null) {
			return Unauthorized("Invalid username");
		}

		using HMACSHA512 hmac = new HMACSHA512(user.PasswordSalt);
		byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userLoginDto.Password));

		for (int i = 0; i < computedHash.Length; i++) {
			if (computedHash[i] != user.PasswordHash[i]) {
				return Unauthorized("Invalid password");
			}
		}

		UserDto? userDto = _mapper.Map<UserDto>(user);

		userDto.Token = _tokenService.CreateToken(user);

		return Ok(userDto);
	}
}