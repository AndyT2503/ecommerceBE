namespace Ecommerce.Application.Services.MailNotifyService
{
    public class MailContent
    {
        public string To { get; set; }
        public object Data { get; set; }
        public string EventCode { get; set; }
    }
}
