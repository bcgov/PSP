using System;
using Model = Pims.Api.Models;

namespace Pims.Api.Areas.Property.Models.Parcel
{
    public class ParcelFiscalModel : Model.BaseAppModel
    {
        #region Properties
        public long ParcelId { get; set; }

        public int FiscalYear { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public string Key { get; set; }

        public decimal Value { get; set; }

        public string Note { get; set; }
        #endregion
    }
}
