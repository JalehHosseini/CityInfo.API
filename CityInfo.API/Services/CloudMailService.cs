namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService
    {

        private readonly string _mailTo = string.Empty;
        private readonly string _mailFrom = string.Empty;

        public CloudMailService(IConfiguration configuration)
        {
            _mailTo = configuration["MailSetting: MailToAddress"];
            _mailFrom = configuration["MailSetting:MailFromAddress"];
        }




        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail  From {_mailFrom}  To {_mailTo}  , "
                + $"with {nameof(CloudMailService)}  ,  ");
            Console.WriteLine($"Subject {subject}");
            Console.WriteLine($"Message {message}");
        }

    }
}
