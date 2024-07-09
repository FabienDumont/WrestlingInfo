using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WrestlingInfo.API.Entities;

namespace WrestlingInfo.API.Services;

public class TokenService : ITokenService {
	private readonly SymmetricSecurityKey _key;
	private readonly string _issuer;
	private readonly string _audience;

	public TokenService(JwtSettings jwtSettings) {
		_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.TokenKey));
		_issuer = jwtSettings.Issuer;
		_audience = jwtSettings.Audience;
	}

	public string CreateToken(User user) {
		List<Claim> claims = new List<Claim> {
			new(JwtRegisteredClaimNames.NameId, user.Username)
		};

		SigningCredentials creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

		SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor {
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.Now.AddDays(7),
			SigningCredentials = creds,
			Issuer = _issuer,
			Audience = _audience
		};

		JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

		SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);

		return tokenHandler.WriteToken(token);
	}
}