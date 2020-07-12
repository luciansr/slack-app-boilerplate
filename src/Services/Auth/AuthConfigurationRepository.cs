using System;
using Models.Config;

namespace Services.Auth
{
    public interface IAuthConfigurationRepository
    {
        string GetTeamTokenAsync(string teamDomain);
        string GetTeamBotTokenAsync(string teamDomain);
        string GetTeamSigningSecretAsync(string teamDomain);
    }

    public class AuthConfigurationRepository : IAuthConfigurationRepository
    {
        private readonly SlackConfig _slackConfig;

        public AuthConfigurationRepository(SlackConfig slackConfig)
        {
            _slackConfig = slackConfig;
        }
        
        public string GetTeamTokenAsync(string teamDomain)
        {
            return _slackConfig.SlackTokens[teamDomain].TeamToken;
        }

        public string GetTeamBotTokenAsync(string teamDomain)
        {
            return _slackConfig.SlackTokens[teamDomain].BotToken;
        }

        public string GetTeamSigningSecretAsync(string teamDomain)
        {
            return _slackConfig.SlackTokens[teamDomain].SigningSecret;
        }
    }
}