using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities;

/// <summary>
/// PimsDspAgreementStatusType class, provides an entity for the datamodel to manage Disposition Agreement Status types.
/// </summary>
public partial class PimsDspAgreementStatusType : ITypeEntity<string>
{
    [NotMapped]
    public string Id { get => DspAgreementStatusTypeCode; set => DspAgreementStatusTypeCode = value; }

    public PimsDspAgreementStatusType()
    {
    }

    public PimsDspAgreementStatusType(string id)
    {
        Id = id;
    }
}
