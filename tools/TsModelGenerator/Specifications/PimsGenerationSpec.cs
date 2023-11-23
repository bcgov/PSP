using Pims.Api.Concepts.Models.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Pims.Api.Models;
using Pims.Api.Models.Concepts;
using TypeGen.Core.Extensions;
using TypeGen.Core.SpecGeneration;

namespace Pims.Tools.TsModelGenerator.Specifications
{
    public class PimsGenerationSpec : GenerationSpec
    {
        public override void OnBeforeGeneration(OnBeforeGenerationArgs args)
        {
            //        AddClass<ProductDto>();
            //
            //        AddInterface<CarDto>("output/directory");

            //AddClass<PersonModel>().CustomHeader("// WELL HELLO THERE\n");
            //.Member(nameof(PersonDto.Id))  // specifying member options
            //.Ignore()
            //.Member(x => nameof(x.Age))    // you can specify member name with lambda
            //.Type(TsType.String);
            //
            //AddInterface<SettingsDto>()
            //.IgnoreBase();                 // specifying type options
            //
            //AddClass(typeof(GenericDto<>));    // specifying types by Type instance
            //
            //AddEnum<ProductType>("output/dir") // specifying an enumv
            //
            // generate everything from an assembly

            //foreach (Type type in GetType().Assembly.GetLoadableTypes())
            //{
            //AddClass(type);
            //}

            // generate types by namespace

            AddInterface<BaseAuditModel>();
            AddInterface<BaseConcurrentModel>();


            var conceptsAssembly = Assembly.Load("Pims.Api.Concepts");

            //var assembly = Assembly.GetCallingAssembly().GetReferencedAssemblies(); // get the assembly to generate files for

            IEnumerable<Type> types = conceptsAssembly.GetLoadableTypes()
                .Where(x => x.FullName.StartsWith("Pims.Api.Concepts.Models.Concepts"));
            foreach (Type type in types)
            {
                System.Console.WriteLine(type.FullName);
                if (type.FullName.EndsWith("Model"))
                {
                    var path = type.FullName.Replace("Pims.Api.Concepts.Models.Concepts", "Concepts.Models.Concepts").Replace(".", "/");
                    AddInterface(type).CustomHeader($"// LINK: @backend/{path}.cs\n");
                }
            }
        }

        public override void OnBeforeBarrelGeneration(OnBeforeBarrelGenerationArgs args)
        {
            //AddBarrel(".", BarrelScope.Files); // adds one barrel file in the global TypeScript output directory containing only files from that directory

            //AddBarrel(".", BarrelScope.Files | BarrelScope.Directories); // equivalent to AddBarrel("."); adds one barrel file in the global TypeScript output directory containing all files and directories from that directory


            // the following code, for each directory, creates a barrel file containing all files and directories from that directory

            IEnumerable<string> directories = GetAllDirectoriesRecursive(args.GeneratorOptions.BaseOutputDirectory)
                .Select(x => GetPathDiff(args.GeneratorOptions.BaseOutputDirectory, x));

            foreach (string directory in directories)
            {
                AddBarrel(directory);
            }

            //AddBarrel(".");
        }

        private string GetPathDiff(string pathFrom, string pathTo)
        {
            var pathFromUri = new Uri("file:///" + pathFrom?.Replace('\\', '/'));
            var pathToUri = new Uri("file:///" + pathTo?.Replace('\\', '/'));

            return pathFromUri.MakeRelativeUri(pathToUri).ToString();
        }

        private IEnumerable<string> GetAllDirectoriesRecursive(string directory)
        {
            var result = new List<string>();
            string[] subdirectories = Directory.GetDirectories(directory);

            if (!subdirectories.Any()) return result;

            result.AddRange(subdirectories);

            foreach (string subdirectory in subdirectories)
            {
                result.AddRange(GetAllDirectoriesRecursive(subdirectory));
            }

            return result;
        }
    }
}
