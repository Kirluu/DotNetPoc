using Serilog.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Serilog
{
    public static class LoggingExtensions
    {
        public static LoggerConfiguration WithCustomLogLevel(
            this LoggerEnrichmentConfiguration enrich)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(enrich));

            return enrich.With<CustomLogLevelEnricher>();
        }
    }
}
