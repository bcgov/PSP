using System;

namespace Pims.Api.Areas.Tools.Models.Import
{
    public class ParcelEvaluationModel : Api.Models.BaseAppModel
    {
        #region Properties
        public long ParcelId { get; set; }

        public DateTime Date { get; set; }

        public string Key { get; set; }

        public decimal Value { get; set; }

        public string Note { get; set; }
        #endregion
    }
}
