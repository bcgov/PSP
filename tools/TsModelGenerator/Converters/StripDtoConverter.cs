using System.Reflection;
using TypeGen.Core.Converters;

namespace Pims.Tools.TsModelGenerator.Converter
{
    public class StripDtoConverter : ITypeNameConverter
    {
        public string Convert(string name, MemberInfo memberInfo)
        {
            return ConvertName(name);
        }

        public string Convert(string name, Type type)
        {
            System.Console.WriteLine(name);
            System.Console.WriteLine("\t -N " + type.ToString());

            return ConvertName(name);
        }

        public string ConvertName(string name)
        {
            var badSufix = "Model";
            var goodPrefix = "Api2_";

            //System.Console.WriteLine(name);
            var a = name.EndsWith(badSufix) ? name.Substring(0, name.Length - badSufix.Length) : name;

            //System.Console.WriteLine(a);

            return goodPrefix + a;
        }
    }
}