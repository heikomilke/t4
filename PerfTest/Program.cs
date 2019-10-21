using System;
using System.Diagnostics;
using Mono.TextTemplating;

namespace PerfTest
{





    class Program
    {
        static void Main(string[] args)
        {


            var gen = new TemplateGenerator();

            Measure.Step();


string outputFilename = "";

            gen.ProcessTemplate(
              null,
              "hi",
              ref outputFilename,
              out var outputContent);
            Measure.Step("Done processing");

        }
    }
}
