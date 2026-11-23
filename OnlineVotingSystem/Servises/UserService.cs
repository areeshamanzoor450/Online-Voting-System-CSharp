using System.Text;
using System.Xml.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using MimeKit;
using OnlineVotingSystem.Entities;
using OnlineVotingSystem.Interfaces;
using OnlineVotingSystem.Servises;
using Org.BouncyCastle.Crypto.Macs;
using SendGrid.Helpers.Mail;

namespace OnlineVotingSystem
{
    public class UserService(AccountService accountService) : IUserService
    {
        public readonly AccountService accountService = accountService;

        public async Task<string> Send(string gmail, string name)
            => await Execute(gmail, name);

        private static async Task<string> Execute(string gmail, string name)
        {
            var subject = "Verification Code";
            var verificationCode = GenerateVerificationCode();
            var plainTextContent = $"Your verification code is: {verificationCode}";
            var htmlContent = $"<p>Your verification code is: <strong>{verificationCode}</strong></p>";

            // Authenticate with Gmail API
            var service = AuthenticateGmail();

            // Send email via Gmail API
            try
            {
                SendEmail(service, gmail, subject, name, plainTextContent, htmlContent);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while sending the verification email via Gmail", ex);
            }

            return verificationCode;
        }

        // Generates a random verification code (6 digits)
        private static string GenerateVerificationCode()
        {
            const int codeLength = 6;
            const string characters = "0123456789";
            var random = new Random();
            var code = new char[codeLength];

            for (int i = 0; i < codeLength; i++)
            {
                code[i] = characters[random.Next(characters.Length)];
            }

            return new string(code);
        }

        // Authenticate with Gmail API using OAuth 2.0
        private static GmailService AuthenticateGmail()
        {
            string[] scopes = { GmailService.Scope.GmailSend };
            var credentialPath = "../../../../OnlineVotingSystem/Credentials/token.json"; // Store token file here 

            // Load client secrets (credentials.json file from Google Cloud Console)
            using (var stream = new FileStream("../../../../OnlineVotingSystem/Credentials/client_secret.json", FileMode.Open, FileAccess.Read))
            {
                var clientSecrets = GoogleClientSecrets.FromStream(stream).Secrets;

                var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    clientSecrets,
                    scopes,
                    "user", // The user needs to authenticate
                    CancellationToken.None,
                    new FileDataStore(credentialPath, true)).Result;

                // Initialize the Gmail API service
                var service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Online Voting System",
                });

                return service;
            }
        }

        // Send email using Gmail API
        private static void SendEmail(GmailService service, string recipientEmail, string subject, string name, string plainTextContent, string htmlContent)
        {
            // Create a MimeMessage
            var message = new MimeMessage();

            // Sender's email address and display name
            message.From.Add(new MimeKit.MailboxAddress("Online Voting System", "YOUR_EMAIL@gmail.com")); 

            // Recipient's email address and display name
        
            message.To.Add(new MimeKit.MailboxAddress(name, recipientEmail)); 

            message.Subject = subject;

            var bodyBuilder = new MimeKit.BodyBuilder();
            bodyBuilder.TextBody = plainTextContent;
            bodyBuilder.HtmlBody = htmlContent;
            message.Body = bodyBuilder.ToMessageBody();

            // Convert MimeMessage to raw format
            var rawMessage = Base64UrlEncode(message.ToString());

            // Send the message via Gmail API
            var emailMessage = new Message
            {
                Raw = rawMessage
            };

            var request = service.Users.Messages.Send(emailMessage, "me");
            request.Execute(); // Send the email
        }


        // Base64Url encoding
        private static string Base64UrlEncode(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", ""); 
        }

       
        public Account GetAccount(string gmail)
            => accountService.GetAccountByGmail(gmail);
    }
}


