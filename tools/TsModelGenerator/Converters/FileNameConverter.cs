using NetTopologySuite.Index.Bintree;
using TypeGen.Core.Converters;

namespace Pims.Tools.TsModelGenerator.Converter
{
    public class FileNameConverter : ITypeNameConverter
    {
        public string Convert(string name, Type typeInfo)
        {
            System.Console.WriteLine(typeInfo.FullName);

            var typeInfoName = string.Empty;
            if (typeInfo.FullName == null)
            {
                typeInfoName = typeInfo.Namespace;
            }
            else
            {
                typeInfoName = typeInfo.FullName;
            }

            var isSystemType = typeInfoName.StartsWith("System");

            string? root;
            if (isSystemType)
            {
                root = "System";
            }
            else
            {
                var tokenized = typeInfoName.Split(".");
                root = tokenized[3];
            }

            var badSufix = "Model";

            var cleanupName = name.EndsWith(badSufix) ? name.Substring(0, name.Length - badSufix.Length) : name;

            cleanupName = root + "_" + cleanupName;

            return "ApiGen_" + cleanupName;
        }
    }
}