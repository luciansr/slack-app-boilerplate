using System;
using System.Collections.Generic;

namespace Models.Config
{
    public class SlackConfig
    {
        public string Tokens { get; set; }

        private Dictionary<string, SlackToken> _slackTokens;
        public Dictionary<string, SlackToken> SlackTokens {
            get
            {
                if (string.IsNullOrWhiteSpace(Tokens))
                {
                    return new Dictionary<string, SlackToken>();
                }

                if (_slackTokens != null)
                {
                    return _slackTokens;
                }
                
                _slackTokens = new Dictionary<string, SlackToken>();

                var tokens = Tokens.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);

                foreach (var token in tokens)
                {
                    var tokenSplit = token.Split(new[] {":"}, StringSplitOptions.None);
                    var tokenKey = tokenSplit[0];
                    var tokenValue = tokenSplit[1];
                    var workspace = tokenSplit[0].Replace("_bot", "").Replace("_signing_secret", "");

                    SlackToken workspaceToken;
                    if (_slackTokens.ContainsKey(workspace))
                    {
                        workspaceToken = _slackTokens[workspace];
                    }
                    else
                    {
                        workspaceToken = new SlackToken
                        {
                            Workspace = workspace
                        };
                        _slackTokens.Add(workspace, workspaceToken);
                    }

                    if (tokenKey.EndsWith("_bot"))
                    {
                        workspaceToken.BotToken = tokenValue;
                    }
                    else if (tokenKey.EndsWith("_signing_secret"))
                    {
                        workspaceToken.SigningSecret = tokenValue;
                    }
                    else //team_token
                    {
                        workspaceToken.TeamToken = tokenValue;
                    }
                }

                return _slackTokens;
            }
        }
    }
}