using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiSample.Options;

namespace CombinedTests.ConfigurationValidation
{
    public class TestOptions
    {
        public required int Number { get; set; }
        public required string Url { get; set; }
        public required string String { get; set; }
        public required InnerTestOptions InnerObject { get; set; }
        public required int[] ArrayOfInts { get; set; }
        public required List<string> ListOfStrings { get; set; }
        public required InnerTestOptions[] ArrayOfInnerObjects { get; set; }
    }

    public class InnerTestOptions
    {
        public required int InnerNumber { get; set; }
        public required string InnerString { get; set; }
    }
}
