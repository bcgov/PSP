using System.Collections.Generic;

namespace Pims.Dal.Entities.Models
{
    public class AcquisitionReportFilterModel
    {
        public IEnumerable<long> Projects { get; set; }

        public IEnumerable<long> AcquisitionTeamPersons { get; set; }
    }
}
