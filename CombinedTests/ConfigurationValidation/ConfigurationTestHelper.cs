using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombinedTests.ConfigurationValidation
{
    public static class ConfigurationTestHelper
    {
        public static IConfiguration GetConfigurationFromJson(string jsonString)
        {
            var byteArray = Encoding.UTF8.GetBytes(jsonString);
            var stream = new MemoryStream(byteArray);
            var config = new ConfigurationBuilder().AddJsonStream(stream).Build();
            return config;
        }
    }
}
