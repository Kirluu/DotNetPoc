using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.Serilog
{
    public class CustomLogLevelEnricher : ILogEventEnricher
    {
        public const string PropertyName = "MyLogLevel";

        /// <summary>
        /// Enrich the log event.
        /// </summary>
        /// <param name="logEvent">The log event to enrich.</param>
        /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var (myLogLevel, foregroundColor, backgroundColor) = GetMyLogLevelString(logEvent.Level);
            var logEventProperty = propertyFactory.CreateProperty(PropertyName, myLogLevel);
            var consoleLogEventProperty = new ConsoleLogEventProperty(logEventProperty, foregroundColor, backgroundColor);
            logEvent.AddPropertyIfAbsent(consoleLogEventProperty);
        }

        private static (string, ConsoleColor, ConsoleColor?) GetMyLogLevelString(LogEventLevel logLevel)
        {
            return logLevel switch
            {
                LogEventLevel.Verbose => ("MYTRACE", ConsoleColor.Blue, null),
                LogEventLevel.Debug => ("MYDEBUG", ConsoleColor.Green, null),
                LogEventLevel.Information => ("MYINFO", ConsoleColor.White, null),
                LogEventLevel.Warning => ("MYWARNING", ConsoleColor.Yellow, null),
                LogEventLevel.Error => ("MYERROR", ConsoleColor.Red, null),
                LogEventLevel.Fatal => ("MYCRITICAL", ConsoleColor.White, ConsoleColor.Red),
                _ => ("MYUNKNOWN", ConsoleColor.White, ConsoleColor.Red)
            };
        }
    }
}
