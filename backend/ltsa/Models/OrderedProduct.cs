using System.Runtime.Serialization;

namespace Pims.Ltsa.Models
{
    public class OrderedProduct<T> : ProductParent
    {
        [DataMember(Name = "fieldedData", EmitDefaultValue = false)]
        public T FieldedData { get; set; }
    }
}
