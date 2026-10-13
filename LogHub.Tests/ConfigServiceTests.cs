using Xunit;
using LogHub;

namespace LogHub.Tests
{
    public class ConfigServiceTests
    {
        // ==========================================================
        // 1. ПРОБЛЕМА КЛАССИЧЕСКОГО СИНГЛТОНА
        // ==========================================================

        [Fact]
        public void ClassicSingleton_Test1_SetsValue()
        {
            var config = ClassicConfigService.Instance();
            config.SetValue("SharedKey", "HelloWorld");
            
            Assert.Equal("HelloWorld", config.GetValue("SharedKey"));
        }
        [Fact(Skip = "Падает в наборе из-за разделяемого статического состояния ClassicConfigService")]
        public void ClassicSingleton_Test2_ExpectsCleanState()
        {
            var config = ClassicConfigService.Instance();
            Assert.Null(config.GetValue("SharedKey"));
        }

        // ==========================================================
        // 2. РЕШЕНИЕ ПРОБЛЕМЫ (Обычный класс для DI-контейнера)
        // ==========================================================

        [Fact]
        public void NormalConfig_Test1_SetsValue()
        {
            var config = new ConfigService();
            config.SetValue("IsolatedKey", "HelloWorld");
            
            Assert.Equal("HelloWorld", config.GetValue("IsolatedKey"));
        }

        [Fact]
        public void NormalConfig_Test2_ExpectsCleanState()
        {
            var config = new ConfigService();
            Assert.Null(config.GetValue("IsolatedKey"));
        }
    }
}