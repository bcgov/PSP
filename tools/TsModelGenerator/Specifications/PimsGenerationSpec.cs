using System.Reflection;
using Pims.Api.Models.Base;
using TypeGen.Core.Extensions;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.SpecGeneration.Builders;

namespace Pims.Tools.TsModelGenerator.Specifications
{
    public class PimsGenerationSpec : GenerationSpec
    {
        public override void OnBeforeGeneration(OnBeforeGenerationArgs args)
        {

            ProcessInterface(typeof(BaseAuditModel));
            ProcessInterface(typeof(BaseConcurrentModel));

            var genericTypeModel = typeof(TypeModel<string>).GetGenericTypeDefinition();
            ProcessInterface(genericTypeModel);

            var modelsAssembly = Assembly.Load("Pims.Api.Models");

            // Get the types only from the specified namespace
            IEnumerable<Type> assemblyTypes = modelsAssembly.GetLoadableTypes()
                .Where(x => x.FullName.StartsWith("Pims.Api.Models.Concepts"));

            IEnumerable<Type> baseTypes = modelsAssembly.GetLoadableTypes()
                .Where(x => x.FullName.StartsWith("Pims.Api.Models.Base."));

            foreach (Type type in baseTypes)
            {
                System.Console.WriteLine(type.FullName);
            }

            foreach (Type type in assemblyTypes)
            {
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
            //   @backend\apimodels\Models\Concepts\Property\{filename} -> Pims.Api.Models.Concepts.Property.{filename}
            //   @backend\Base\{filename}           -> Pims.Api.Models.Base.{filename}
            //   @backend\Concepts\Lease\{filename} -> Pims.Api.Models.Concepts.Lease.{filename}
            var path = type.FullName.Replace("Pims.Api", "apimodels").Replace(".", "/");

            // Generic types have a different namespace termination. Remove it if found.
            int index = path.IndexOf("`");
            if (index >= 0)
            {
                path = path.Substring(0, index);
            }

            var linkHeader = $"\n// LINK: @backend/{path}.cs\n";
            var interfaceBuilder = AddInterface(type).CustomHeader(linkHeader);

            //System.Console.WriteLine(type.FullName);

            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var memberBuilder = interfaceBuilder.Member(property.Name);

                memberBuilder = ProcessNullable(memberBuilder, property);
                memberBuilder = ProcessDateTime(memberBuilder, property);
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
                //builder.Type("utcIsoDateTime", "@/models/api/UtcIsoDateTime"); Enable when the datetime front end type has been created.
                builder.Type("string");

            }

            return builder;
        }

        public override void OnBeforeBarrelGeneration(OnBeforeBarrelGenerationArgs args)
        {
            AddBarrel(".", BarrelScope.Files); // adds one barrel file in the global TypeScript output directory containing only files from that directory
        }
    }
}
