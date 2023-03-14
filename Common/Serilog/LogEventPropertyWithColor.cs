using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Serilog
{
    class ConsoleLogEventPropertyValue : LogEventPropertyValue
    {
        private readonly LogEventPropertyValue Value;
        private readonly ConsoleColor ForegroundColor;
        private readonly ConsoleColor? BackgroundColor;

        public ConsoleLogEventPropertyValue(LogEventPropertyValue value, ConsoleColor foregroundColor, ConsoleColor? backgroundColor)
        {
            Value = value;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }

        public override void Render(TextWriter output, string format = null, IFormatProvider formatProvider = null)
        {
            var originalForegroundColor = Console.ForegroundColor;
            var originalBackgroundColor = Console.BackgroundColor;
            Console.ForegroundColor = ForegroundColor;
            if (BackgroundColor is not null)
                Console.BackgroundColor = (ConsoleColor) BackgroundColor;

            Value.Render(output, format, formatProvider); // TODO: For whatever reason, the log-level value gets "quoted" (like that). As described in https://github.com/serilog/serilog-sinks-console/issues/101

            if (BackgroundColor is not null)
                Console.BackgroundColor = originalBackgroundColor;
            Console.ForegroundColor = originalForegroundColor;
        }
    }

    class ConsoleLogEventProperty : LogEventProperty
    {
        public ConsoleLogEventProperty(LogEventProperty property, ConsoleColor foregroundColor, ConsoleColor? backgroundColor)
            : base(property.Name, new ConsoleLogEventPropertyValue(property.Value, foregroundColor, backgroundColor))
        {
        }
    }
}
