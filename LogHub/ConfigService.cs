using System.Collections.Generic;

namespace LogHub
{
    public class ConfigService : IConfigService
    {
        private readonly Dictionary<string, string> _settings = new Dictionary<string, string>();

        public string GetValue(string key)
        {
            return _settings.TryGetValue(key, out var value) ? value : null;
        }

        public void SetValue(string key, string value)
        {
            _settings[key] = value;
        }
    }
}