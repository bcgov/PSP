using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Keyless]
    [Table("TMP_PIMS_PROP_PROP_ADJACENT_LAND_TYPE")]
    public partial class TmpPimsPropPropAdjacentLandType
    {
        [Column("PROPERTY_ID")]
        public long? PropertyId { get; set; }
        [Column("PROPERTY_ADJACENT_LAND_TYPE_CODE")]
        [StringLength(50)]
        public string PropertyAdjacentLandTypeCode { get; set; }
    }
}
