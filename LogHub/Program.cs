using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace LogHub
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Читаем профиль из файла (console или file)
            // Если файла нет, по умолчанию будет console
            string profile = "console";
            if (File.Exists("profile.txt"))
            {
                profile = File.ReadAllText("profile.txt").Trim().ToLower();
            }

            // 2. Создаем контейнер зависимостей
            var services = new ServiceCollection();

            // Регистрируем сервис конфигурации
            services.AddSingleton<IConfigService, ConfigService>();

            // Регистрируем базовые слои процессора (каждый раз создается новый экземпляр - Transient)
            services.AddTransient<ILogParser, SimpleParser>();
            services.AddTransient<ILogFilter>(sp => new SeverityFilter(LogSeverity.Info));
            services.AddTransient<ILogReader, DummyReader>();

            // 3. Подмена реализации через конфигурацию (Пункт 4 из методички)
            if (profile == "file")
            {
                services.AddTransient<ILogSink, FileSink>();
            }
            else
            {
                services.AddTransient<ILogSink, ConsoleSink>();
            }

            // Регистрируем сам процессор
            services.AddTransient<LogProcessor>();

            // 4. СТРОИМ ПРОВАЙДЕР (Композиционный корень закрывается)
            using var serviceProvider = services.BuildServiceProvider();

            // ЕДИНСТВЕННЫЙ вызов GetRequiredService во всём приложении!
            var processor = serviceProvider.GetRequiredService<LogProcessor>();
            
            Console.WriteLine($"--- Запуск с профилем: {profile} ---");
            processor.ProcessLogs();
        }
    }

    // ====================================================================
    // Простые реализации приёмников и читателя, чтобы программа работала
    // ====================================================================
    public class DummyReader : ILogReader
    {
        public System.Collections.Generic.IEnumerable<string> Read()
        {
            yield return "Error: Disk failure";
            yield return "Warning: High memory usage";
        }
    }

    public class ConsoleSink : ILogSink
    {
        public void Write(LogEntry entry) => Console.WriteLine($"[Вывод в КОНСОЛЬ] {entry.Severity}: {entry.Message}");
    }

    public class FileSink : ILogSink
    {
        public void Write(LogEntry entry) => Console.WriteLine($"[Вывод в ФАЙЛ] (имитация записи) {entry.Severity}: {entry.Message}");
    }
}