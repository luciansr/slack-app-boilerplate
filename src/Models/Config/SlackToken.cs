using System.Security.Cryptography.X509Certificates;

namespace Models.Config
{
    public class SlackToken
    {
        public string Workspace { get; set; }
        public string TeamToken { get; set; }
        public string BotToken { get; set; }
        public string SigningSecret { get; set; }
    }
}