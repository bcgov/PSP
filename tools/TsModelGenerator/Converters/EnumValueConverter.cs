using System.Reflection;
using System.Runtime.Serialization;
using TypeGen.Core.Converters;

namespace Pims.Tools.TsModelGenerator.Converter
{
    public class EnumValueConverter : IMemberNameConverter
    {
        public string Convert(string name, MemberInfo memberInfo)
        {
            // Currently the json converter is not being used when serializing. Uncomment the section below once the EnumName attribute is used for serialization to json. 
            var enumAttribute = (EnumMemberAttribute?)memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false).FirstOrDefault();

            if (enumAttribute != null)
            {
                return enumAttribute.Value;
            }

            return name;
        }
    }
}