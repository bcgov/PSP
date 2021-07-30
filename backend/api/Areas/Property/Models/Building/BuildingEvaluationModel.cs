using System;
using Model = Pims.Api.Models;

namespace Pims.Api.Areas.Property.Models.Building
{
    public class BuildingEvaluationModel : Model.BaseAppModel
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
