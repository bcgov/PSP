using System.Collections.Generic;
using System.Linq;
using Pims.Core.Extensions;
using Pims.Dal.Entities;

namespace Pims.Api.Areas.Reports.Models.Lease
{
    public class AggregatedLeasesModel
    {
        public AggregatedLeasesModel(IEnumerable<PimsLease> leases, int fiscalYear, IEnumerable<PimsLeaseProgramType> programs, IEnumerable<PimsRegion> regions)
        {
            this.FiscalYear = fiscalYear.FiscalYear();
            AggregatedLeaseRegions = regions.Select(r => new AggregatedLeaseModel(leases.Where(l => l.RegionCode == r.RegionCode), fiscalYear, region: r.RegionName));
            AggregatedLeasePrograms = programs.Select(p => new AggregatedLeaseModel(leases.Where(l => l.LeaseProgramTypeCode == p.Id), fiscalYear, program: p.Description));
        }

        public string FiscalYear { get; set; }

        public IEnumerable<AggregatedLeaseModel> AggregatedLeaseRegions { get; set; }

        public IEnumerable<AggregatedLeaseModel> AggregatedLeasePrograms { get; set; }
    }
}
