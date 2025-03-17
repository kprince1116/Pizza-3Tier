using System.Net;
using System.Net.Mail;
using BAL.Interfaces;

namespace BAL.Services;

public class EmailService : IEmailService
{
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            var fromAddress = new MailAddress("test.dotnet@etatvasoft.com", "PIZZASHOP");
            var toAddress = new MailAddress(email);
            const string fromPassword = "P}N^{z-]7Ilp";

            var smtp = new SmtpClient
            {
                Host = "mail.etatvasoft.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                IsBodyHtml = true,
                Body = body ,
            };

             smtp.Send(message);
        }

    public async Task SendResetEmail(string email, string resetUrl)
    {
         var fromAddress = new MailAddress("test.dotnet@etatvasoft.com", "PIZZASHOP");
            var toAddress = new MailAddress(email);
            const string fromPassword = "P}N^{z-]7Ilp";
            const string subject = "Password Reset Request";

            var smtp = new SmtpClient
            {
                Host = "mail.etatvasoft.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                IsBodyHtml = true,
                Body = $@" 
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Reset Password - PIZZASHOP</title>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        margin: 0;
                        padding: 0;
                        height: 100vh;
                    }}
                    .container {{
                        background-color: white;
                        width: 750px;
                        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                        border-radius: 8px;
                        overflow: hidden;
                    }}
                    .header {{
                        background-color: #0d5e99;
                        color: white;
                        text-align: center;
                        padding: 15px;
                        font-size: 24px;
                        display: flex;
                        justify-content: center;
                        align-items: center;
                    }}
                    .content {{
                        padding: 20px;
                    }}
                    .content a {{
                        color: #0d5e99;
                        text-decoration: none;
                        font-weight: bold;
                    }}
                    .content a:hover {{
                        text-decoration: underline;
                    }}
                    .important-note {{
                        background-color: #fff5cc;
                        padding: 10px;
                        border-left: 5px solid #ffcc00;
                        font-size: 14px;
                        margin-top: 10px;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <img src='~/images/logos/pizzashop_logo.png' alt='logo' style='max-height: 120px; max-width: 120px;'>
                        <h1>PIZZASHOP</h1>
                    </div>
                    <div class='content'>
                        <p><strong>Pizza shop,</strong></p>
                        <p>Please click <a href='{resetUrl}'>Reset Password</a> to reset your account password.</p>
                        <p>If you encounter any issues or have any questions, please do not hesitate to contact our support team.</p>
                        <p class='important-note'><strong>Important Note:</strong> For security reasons, the link will expire in 24 hours. If you did not request a password reset, please ignore this email or contact our support team immediately.</p>
                    </div>
                </div>
            </body>
            </html>"
            };

            smtp.Send(message);
    } 

}
