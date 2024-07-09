using WrestlingInfo.API.Entities;

namespace WrestlingInfo.API.Services;

public interface ITokenService {
	string CreateToken(User user);
}