using Microsoft.AspNetCore.Identity.UI.Services;

namespace VeynVoyage.Services.Implementations
{
	public class DummyEmailSender : IEmailSender
	{
		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			
			return Task.CompletedTask;
		}
	}
}
