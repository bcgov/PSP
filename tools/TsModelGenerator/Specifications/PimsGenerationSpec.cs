using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Pims.Api.Models.Base;
using Pims.Api.Models.Mayan;
using TypeGen.Core.Extensions;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.SpecGeneration.Builders;
using Pims.Api.Models.Concepts.File;
using Pims.Api.Models.Concepts.ResearchFile;
using Pims.Api.Models.Concepts.DispositionFile;

namespace Pims.Tools.TsModelGenerator.Specifications
{
    public class PimsGenerationSpec : GenerationSpec
    {
        public override void OnBeforeGeneration(OnBeforeGenerationArgs args)
        {

            ProcessInterface(typeof(BaseAuditModel));
            ProcessInterface(typeof(BaseConcurrentModel));

            var genericTypeModel = typeof(CodeTypeModel<>);
            ProcessInterface(genericTypeModel);

            var modelsAssembly = Assembly.Load("Pims.Api.Models");

            var genericFileModel = typeof(FileModel);
            ProcessInterface(genericFileModel);


            var dispositionFileModel3 = typeof(FilePropertyModel);
            ProcessInterface(dispositionFileModel3);

            var dispositionFileModel5 = typeof(FileWithChecklistModel);
            ProcessInterface(dispositionFileModel5);
            var dispositionFileModel2 = typeof(ResearchFileModel);
            ProcessInterface(dispositionFileModel2);

            // Get the types only from the specified namespace
            IEnumerable<Type> conceptTypes = modelsAssembly.GetLoadableTypes()
                .Where(x => x.FullName.StartsWith("Pims.Api.Models.Concepts"));

            IEnumerable<Type> requestTypes = modelsAssembly.GetLoadableTypes()
                .Where(x => x.FullName.StartsWith("Pims.Api.Models.Request"));

            IEnumerable<Type> baseTypes = modelsAssembly.GetLoadableTypes()
                .Where(x => x.FullName.StartsWith("Pims.Api.Models.Base"));

            IEnumerable<Type> mayanTypes = modelsAssembly.GetLoadableTypes()
                .Where(x => x.FullName.StartsWith("Pims.Api.Models.Mayan"));

            var genericMayanResponseTypeModel = typeof(QueryResponse<string>).GetGenericTypeDefinition();
            ProcessInterface(genericMayanResponseTypeModel);

            IEnumerable<Type> codeTypes = modelsAssembly.GetLoadableTypes()
                .Where(x => x.FullName.StartsWith("Pims.Api.Models.CodeTypes"));


            foreach (Type type in baseTypes)
            {
                System.Console.WriteLine(type.FullName);
            }

            foreach (Type type in requestTypes)
            {
                //System.Console.WriteLine(type.FullName);
                ProcessInterface(type);
            }

            foreach (Type type in conceptTypes)
            {
                if (type.FullName.EndsWith("Model"))
                {
                    if (type.IsGenericType)
                    {
                        ProcessInterface(type.GetGenericTypeDefinition());
                    }
                    else
                    {
                        ProcessInterface(type);
                    }
                }
            }

            foreach (Type type in mayanTypes)
            {
                if (type.FullName.EndsWith("Model"))
                {
                    ProcessInterface(type);
                }
            }

            foreach (Type type in codeTypes)
            {
                if (type.BaseType == typeof(System.Enum))
                {
                    ProcessEnum(type);
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

                memberBuilder = ProcessJsonAttributes(memberBuilder, property);
                memberBuilder = ProcessNullable(memberBuilder, property);
                memberBuilder = ProcessDateTime(memberBuilder, property);
                memberBuilder = ProcessDateOnly(memberBuilder, property);
                memberBuilder = ProcessFile(memberBuilder, property);
            }

            return interfaceBuilder;
        }

        private EnumSpecBuilder ProcessEnum(Type type)
        {
            var enumBuilder = AddEnum(type).StringInitializers(true);

            var properties = type.GetFields();
            foreach (var property in properties)
            {
                var enumAttribute = (EnumMemberAttribute?)property.GetCustomAttributes(typeof(EnumMemberAttribute), false).FirstOrDefault();

                if (enumAttribute != null)
                {
                    enumBuilder.Member(enumAttribute.Value);
                }

            }

            return enumBuilder;
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

        private InterfaceSpecBuilder ProcessDateOnly(InterfaceSpecBuilder builder, PropertyInfo propertyInfo)
        {
            // Convert DateOnly to custom date definition
            if (propertyInfo.PropertyType == typeof(System.DateOnly?) || propertyInfo.PropertyType == typeof(System.DateOnly))
            {
                //builder.Type("utcIsoDateOnly", "@/models/api/UtcIsoDateOnly"); Enable when the datetime front end type has been created.
                builder.Type("string");

            }

            return builder;
        }

        private InterfaceSpecBuilder ProcessFile(InterfaceSpecBuilder builder, PropertyInfo propertyInfo)
        {
            // Convert IFormFile to File(Blob) type for TS
            if (propertyInfo.PropertyType == typeof(IFormFile))
            {
                builder.NotNull().Type("File");
            }

            return builder;
        }

        private InterfaceSpecBuilder ProcessJsonAttributes(InterfaceSpecBuilder builder, PropertyInfo propertyInfo)
        {
            var jsonPropertyAttribute = (JsonPropertyNameAttribute?)propertyInfo.GetCustomAttributes(typeof(JsonPropertyNameAttribute), false).FirstOrDefault();

            if (jsonPropertyAttribute != null)
            {
                builder.MemberName(jsonPropertyAttribute.Name);
            }
            return builder;
        }

        public override void OnBeforeBarrelGeneration(OnBeforeBarrelGenerationArgs args)
        {
            //AddBarrel(".", BarrelScope.Files); // adds one barrel file in the global TypeScript output directory containing only files from that directory
        }
    }
}
