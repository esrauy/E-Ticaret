using System.Net;
using System.Net.Mail;

namespace ETicaret.WebUI.EmailServices
{
    public class EmailSender : IEmailSender
    {
        // Bu değişkenler mail göndermek için SMTP Server'ının ihtiyacı olan değişkenler (SMTP Server= Mail gönderen server)
        private string _host;
        private int _port;
        private string _username;
        private string _password;
        public bool _enableSSL;

        public EmailSender(string host, int port, bool enableSSL, string userName, string password)
        {
            this._host = host;
            this._port = port;
            this._username = userName;
            this._password = password;
            this._enableSSL = enableSSL;
        }
        
        // email: kime göndereceğiz to
        // subject: konusu
        // htmlMessage: mesaj içeriği
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(this._host, this._port)
            {
                Credentials = new NetworkCredential(_username, _password),
                EnableSsl = this._enableSSL
            };
            return client.SendMailAsync(
                new MailMessage(this._username, email, subject, htmlMessage)
                {
                    IsBodyHtml = true
                });
        }
    }
}
