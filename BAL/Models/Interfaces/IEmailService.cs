namespace BAL.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string email ,  string subject , string body);   

    Task SendResetEmail(string email, string resetUrl);
}
