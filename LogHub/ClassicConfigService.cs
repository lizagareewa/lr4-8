using System.Collections.Generic;

namespace LogHub
{
    public class ClassicConfigService : IConfigService
    {
        private static ClassicConfigService _instance;
        private readonly Dictionary<string, string> _settings = new Dictionary<string, string>();

        // Приватный конструктор
        private ClassicConfigService() { }

        // Глобальная точка доступа
        public static ClassicConfigService Instance()
        {
            if (_instance == null)
            {
                _instance = new ClassicConfigService();
            }
            return _instance;
        }

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