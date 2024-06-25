namespace WrestlingInfo.API.Services;

public class LocalMailService : IMailService {
	private string _mailTo;
	private string _mailFrom;

	public LocalMailService(IConfiguration configuration) {
		_mailTo = configuration["MailSettings:MailToAddress"] ?? throw new InvalidOperationException();
		_mailFrom = configuration["MailSettings:MailFromAddress"] ?? throw new InvalidOperationException();
	}

	public void Send(string subject, string message) {
		Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with {nameof(LocalMailService)}");
		Console.WriteLine($"Subject: {subject}");
		Console.WriteLine($"Message: {message}");
	}
}