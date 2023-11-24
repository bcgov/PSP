using TypeGen.Core.Converters;

namespace Pims.Tools.TsModelGenerator.Converter
{
    public class TypeNameConverter : ITypeNameConverter
    {
        public string Convert(string name, Type type)
        {
            var badSufix = "Model";
            var goodPrefix = "Api2_";

            return goodPrefix + (name.EndsWith(badSufix) ? name.Substring(0, name.Length - badSufix.Length) : name);
        }
    }
}