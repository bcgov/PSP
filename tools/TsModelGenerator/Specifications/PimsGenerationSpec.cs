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
using Namotion.Reflection;
using NetTopologySuite.LinearReferencing;
using TypeGen.Core.SpecGeneration.Builders;

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

            //AddInterface<BaseAuditModel>();
            //AddInterface<BaseConcurrentModel>();
            ProcessInterface(typeof(BaseAuditModel));
            ProcessInterface(typeof(BaseConcurrentModel));

            var conceptsAssembly = Assembly.Load("Pims.Api.Concepts");

            // Get the types only from the specified namespace
            IEnumerable<Type> assemblyTypes = conceptsAssembly.GetLoadableTypes()
                .Where(x => x.FullName.StartsWith("Pims.Api.Concepts.Models.Concepts"));

            foreach (Type type in assemblyTypes)
            {
                System.Console.WriteLine(type.FullName);
                if (type.FullName.EndsWith("Model"))
                {
                    ProcessInterface(type);
                }
            }
        }

        private InterfaceSpecBuilder ProcessInterface(Type type)
        {
            // Add the path to the file based on the namespace.
            // e.g:
            //   @backend\Base\           -> Pims.Api.Concepts.Models.Base.
            //   @backend\Concepts\Lease\ -> Pims.Api.Concepts.Models.Concepts.Lease.
            var path = type.FullName.Replace("Pims.Api.Concepts.Models", "Concepts.Models").Replace(".", "/");
            var interfaceBuilder = AddInterface(type).CustomHeader($"// LINK: @backend/{path}.cs\n");

            var members = type.GetProperties();
            foreach (var lel in members)
            {
                var memberBuilder = interfaceBuilder.Member(lel.Name);

                memberBuilder = ProcessNullable(memberBuilder, lel);
                memberBuilder = ProcessDateTime(memberBuilder, lel);
                memberBuilder.DefaultTypeOutput("./GeneratedTS/" + path);
            }

            return interfaceBuilder;
        }

        private InterfaceSpecBuilder ProcessNullable(InterfaceSpecBuilder builder, PropertyInfo propertyInfo)
        {
            // Add the null union type if the member is nullable
            if (Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null || !propertyInfo.PropertyType.IsValueType)
            {
                builder.TypeUnions(["null"]);
            }

            return builder;
        }

        private InterfaceSpecBuilder ProcessDateTime(InterfaceSpecBuilder builder, PropertyInfo propertyInfo)
        {
            // Convert DateTime to custom date definition
            if (propertyInfo.PropertyType == typeof(System.DateTime?) || propertyInfo.PropertyType == typeof(System.DateTime))
            {
                builder.Type("utcIsoDateTime", "@/models/api/UtcIsoDateTime");
            }

            return builder;
        }

        public override void OnBeforeBarrelGeneration(OnBeforeBarrelGenerationArgs args)
        {
            AddBarrel(".", BarrelScope.Files); // adds one barrel file in the global TypeScript output directory containing only files from that directory

            //AddBarrel(".", BarrelScope.Files | BarrelScope.Directories); // equivalent to AddBarrel("."); adds one barrel file in the global TypeScript output directory containing all files and directories from that directory


            // the following code, for each directory, creates a barrel file containing all files and directories from that directory

            /*IEnumerable<string> directories = GetAllDirectoriesRecursive(args.GeneratorOptions.BaseOutputDirectory)
                .Select(x => GetPathDiff(args.GeneratorOptions.BaseOutputDirectory, x));

            foreach (string directory in directories)
            {
                AddBarrel(directory);
            }*/

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
