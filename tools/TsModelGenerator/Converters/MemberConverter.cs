using System.Reflection;

using DocumentFormat.OpenXml.Drawing;
using TypeGen.Core.Converters;

namespace Pims.Tools.TsModelGenerator.Converter
{
    public class MemberConverter : ITypeNameConverter
    {
        public string Convert(string name, Type typeInfo)
        {
            System.Console.WriteLine(name);
            System.Console.WriteLine("\t -N " + typeInfo.ToString());



            return name;
        }
    }
}