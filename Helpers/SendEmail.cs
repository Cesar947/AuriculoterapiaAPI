using MailKit.Net.Smtp;
using MimeKit;

namespace Auriculoterapia.Api.Helpers
{
    public class SendEmail
    {
        public void sendEmailTo(string nombrePaciente,string emailUserTo, string subject, string textBody){
            
            MimeMessage message = new MimeMessage();
            MailboxAddress from = new MailboxAddress("AuriculoterapiaApp","auriculoterapiaapp@gmail.com");
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress(nombrePaciente,emailUserTo);
            message.To.Add(to);

            message.Subject = subject;

            BodyBuilder bodyBuilder = new BodyBuilder();

            bodyBuilder.TextBody = textBody;

            message.Body = bodyBuilder.ToMessageBody();

            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com",587,false);
            client.Authenticate("auriculoterapiaapp@gmail.com","TPAuriculoterapia123");

            client.Send(message);
            client.Disconnect(true);
            client.Dispose();

        }
    }
}