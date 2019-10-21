﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;

namespace CompileTest
{
    class Program
    {

        const string programText =
@"using System;
using System.Collections;
using System.Linq;
using System.Text;
 
namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, World!"");
        }
    }
}";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //https://luisfsgoncalves.wordpress.com/2017/03/20/referencing-system-assemblies-in-roslyn-compilations/

            var trustedAssembliesPaths = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")).Split(Path.PathSeparator);
            foreach (var tp in trustedAssembliesPaths)
            {
                Console.WriteLine(tp);
            }


        https://josephwoodward.co.uk/2016/12/in-memory-c-sharp-compilation-using-roslyn

            SyntaxTree tree = CSharpSyntaxTree.ParseText(programText);



            var neededAssemblies = new[]
{
    "System.Runtime",
    "mscorlib",
};
            var references = trustedAssembliesPaths
                .Where(p => neededAssemblies.Contains(Path.GetFileNameWithoutExtension(p)))
                .Select(p => MetadataReference.CreateFromFile(p))
                .ToList();
            var ass = new List<MetadataReference>(references);
            ass.Add(MetadataReference.CreateFromFile(typeof(Object).Assembly.Location));
            ass.Add(MetadataReference.CreateFromFile(typeof(System.Linq.Enumerable).Assembly.Location));
             ass.Add(MetadataReference.CreateFromFile(typeof(System.String).Assembly.Location));
             ass.Add(MetadataReference.CreateFromFile(typeof(Console).Assembly.Location));
             ass.Add(MetadataReference.CreateFromFile(typeof(IntPtr).Assembly.Location));
            // 
            var compile = Microsoft.CodeAnalysis.CSharp.CSharpCompilation.Create("fucker",
            options:
                new CSharpCompilationOptions(OutputKind.ConsoleApplication)
            ).AddSyntaxTrees(tree).AddReferences(ass);

            using (var stream = File.OpenWrite("t:\\test.dll"))
            {
                var result = compile.Emit(stream);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}, {2}", diagnostic.Id, diagnostic.GetMessage(), diagnostic.Location);
                    }
                }

            }

        }
    }
}
