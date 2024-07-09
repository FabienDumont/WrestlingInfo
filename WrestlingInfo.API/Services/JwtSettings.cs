namespace WrestlingInfo.API.Services;

public class JwtSettings {
	public string TokenKey { get; set; }
	public string Issuer { get; set; }
	public string Audience { get; set; }
}