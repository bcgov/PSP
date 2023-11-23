using System.Reflection;
using Namotion.Reflection;
using Pims.Tools.TsModelGenerator.Converter;
using Pims.Tools.TsModelGenerator.Specifications;
using TypeGen.Core;
using TypeGen.Core.Extensions;
using TypeGen.Core.Generator;

namespace Pims.Tools.TsModelGenerator
{
    public class TsGenerator
    {
        public void Generate()
        {
            var a = new PimsGenerationSpec();
            var b = new StripDtoConverter();
            var c = new MemberConverter();

            //Pims.Api.Concepts.dll
            var options = new GeneratorOptions { BaseOutputDirectory = @"./GeneratedTS", }; // create the options object
            options.TypeNameConverters.Add(b);
            //options.CustomTypeMappings["System.DateTime"] = "dateISO";
            options.TypeUnionsForTypes["Object"] = ["DDAATEISO"];
            //options.PropertyNameConverters.Add(c);
            options.CsNullableTranslation = StrictNullTypeUnionFlags.Null;
            //options.CsAllowNullsForAllTypes = true;

            var generator = new Generator(options); // create the generator instance


            var assembly = Assembly.GetCallingAssembly(); // get the assembly to generate files for

            var result = generator.Generate(a);

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