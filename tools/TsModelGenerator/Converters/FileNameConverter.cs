using NetTopologySuite.Index.Bintree;
using TypeGen.Core.Converters;

namespace Pims.Tools.TsModelGenerator.Converter
{
    public class FileNameConverter : ITypeNameConverter
    {
        public string Convert(string name, Type typeInfo)
        {
            System.Console.WriteLine(typeInfo.FullName);


            var isSystemType = typeInfo.FullName.StartsWith("System");

            string? root;
            if (isSystemType)
            {
                root = "System";
            }
            else
            {
                var tokenized = typeInfo.FullName.Split(".");
                root = tokenized[3];
            }

            var badSufix = "Model";

            var cleanupName = name.EndsWith(badSufix) ? name.Substring(0, name.Length - badSufix.Length) : name;

            cleanupName = root + "_" + cleanupName;

            return "ApiGen_" + cleanupName;
        }
    }
}