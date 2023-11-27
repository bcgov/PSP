using TypeGen.Core.Converters;

namespace Pims.Tools.TsModelGenerator.Converter
{
    public class FileNameConverter : ITypeNameConverter
    {
        public string Convert(string name, Type typeInfo)
        {
            var badSufix = "Model";
            return "ApiGen_" + (name.EndsWith(badSufix) ? name.Substring(0, name.Length - badSufix.Length) : name);
        }
    }
}