namespace SchoolMgmtAPI.Models
{
    public class SMTPSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
    }
}
