using System;
using System.Collections.Generic;

namespace LogHub
{
    public class LazyConfigService : IConfigService
    {
        private static readonly Lazy<LazyConfigService> _lazy = 
            new Lazy<LazyConfigService>(() => new LazyConfigService());
            
        private readonly Dictionary<string, string> _settings = new Dictionary<string, string>();

        private LazyConfigService() { }

        public static LazyConfigService Instance => _lazy.Value;

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