using System;

namespace Services.Auth
{
    public interface IAuthConfigurationRepository
    {
        string GetTeamTokenAsync(string teamDomain);
        string GetTeamBotTokenAsync(string teamDomain);
        string GetTeamSigningSecretAsync(string teamDomain);
    }

    public class AuthAuthConfigurationRepository : IAuthConfigurationRepository
    {
        public AuthAuthConfigurationRepository()
        {
            
        }
        
        public string GetTeamTokenAsync(string teamDomain)
        {
            throw new NotImplementedException();
        }

        public string GetTeamBotTokenAsync(string teamDomain)
        {
            throw new NotImplementedException();
        }

        public string GetTeamSigningSecretAsync(string teamDomain)
        {
            throw new NotImplementedException();
        }
    }
}