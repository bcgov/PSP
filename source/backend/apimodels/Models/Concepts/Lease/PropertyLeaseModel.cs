using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.File;

namespace Pims.Api.Models.Concepts.Lease
{
    public class PropertyLeaseModel : FilePropertyModel
    {
        #region Properties

        public new long FileId { get; set; }

        public new LeaseModel File { get; set; }

        public double? LeaseArea { get; set; }

        public CodeTypeModel<string> AreaUnitType { get; set; }

        #endregion
    }
}
