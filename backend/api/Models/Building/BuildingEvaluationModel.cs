using System;

namespace Pims.Api.Models.Building
{
    public class BuildingEvaluationModel : BaseAppModel
    {
        #region Properties
        public long BuildingId { get; set; }

        public DateTime Date { get; set; }

        public string Key { get; set; }

        public decimal Value { get; set; }

        public string Note { get; set; }
        #endregion
    }
}
