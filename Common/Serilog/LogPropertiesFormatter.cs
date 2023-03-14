using Serilog.Events;
using Serilog.Formatting;

namespace Common.Serilog
{
    public class LogPropertiesFormatter : ITextFormatter
    {
        public void Format(LogEvent logEvent, TextWriter output)
        {
            var logProperties = string.Join("; ", logEvent.Properties.Select(p => $"{p.Key}={p.Value}"));

            logEvent.RenderMessage(output);
            output.WriteLine(logProperties);
        }
    }
}