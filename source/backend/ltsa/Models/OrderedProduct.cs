using System.Runtime.Serialization;

namespace Pims.Ltsa.Models
{
    public class OrderedProduct<T> : ProductParent<T> where T : IFieldedData
    {
        [DataMember(Name = "fieldedData", EmitDefaultValue = false)]
        public override T FieldedData { get; set; }
    }
}
