using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Config
{
    public class SlackConfig
    {
        public string Tokens { get; set; }
        
        private Dictionary<string, string> _tokenMap;
        public Dictionary<string, string> TokenMap {
            get
            {
                if (string.IsNullOrWhiteSpace(Tokens))
                {
                    return new Dictionary<string, string>();
                }

                return _tokenMap ?? (
                   _tokenMap = Tokens
                       .Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries)
                       .Select(x => x.Split(new[] {":"}, StringSplitOptions.None))
                       .ToDictionary(x => x[0], x => x[1])
                );
            }
        }
    }
}