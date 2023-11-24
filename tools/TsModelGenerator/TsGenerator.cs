using System.Reflection;
using Namotion.Reflection;
using Pims.Tools.TsModelGenerator.Converter;
using Pims.Tools.TsModelGenerator.Specifications;
using TypeGen.Core;
using TypeGen.Core.Converters;
using TypeGen.Core.Extensions;
using TypeGen.Core.Generator;

namespace Pims.Tools.TsModelGenerator
{
    public class TsGenerator
    {
        public void Generate()
        {
            var generationSpec = new PimsGenerationSpec();

            var typeNameConverter = new TypeNameConverter();
            var fileNameConverter = new FileNameConverter();

            //var frontendApiPath = "../../source/frontend/src/models/api/generated";
            var frontendApiPath = "./GeneratedTS";
            var options = new GeneratorOptions();
            //options.BaseOutputDirectory = $"{frontendApiPath}";
            options.TypeNameConverters.Add(typeNameConverter);
            options.FileNameConverters = [fileNameConverter, new PascalCaseToKebabCaseConverter()];
            options.TabLength = 2;
            options.FileHeading = "/*\n  Welcome, to the Construct. We can load anything...\n  Anything we need...\n  Welcome to the desert of the _real_.\n*/\n";


            var generator = new Generator(options); // create the generator instance

            var assembly = Assembly.GetCallingAssembly(); // get the assembly to generate files for

            var result = generator.Generate(generationSpec);

            Console.WriteLine("--- START ---");
            foreach (var line in result)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine("--- END ---");


            /*IEnumerable<Type> types = GetType().Assembly.GetLoadableTypes()
               .Where(x => x.FullName.StartsWith("Pims.Api.Models"));
           foreach (Type type in types)
           {
               AddClass(type);
           }*/
        }
    }
}