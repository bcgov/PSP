using System;

namespace Pims.Api.Models.Concepts.Property
{
    /// <summary>
    /// PropertyViewModel class, provides a model to represent the property view.
    /// </summary>
    public class PropertyViewModel
    {
        #region Properties

        public long Id { get; set; }

        public int? Pid { get; set; }

        public string PidPadded { get; set; }

        public int? Pin { get; set; }

        public string PropertyTypeCode { get; set; }

        public string PropertyStatusTypeCode { get; set; }

        public string PropertyDataSourceTypeCode { get; set; }

        public DateOnly PropertyDataSourceEffectiveDate { get; set; }

        public string PropertyClassificationTypeCode { get; set; }

        public string PropertyTenureTypeCode { get; set; }

        public string StreetAddress1 { get; set; }

        public string StreetAddress2 { get; set; }

        public string StreetAddress3 { get; set; }

        public string MunicipalityName { get; set; }

        public string PostalCode { get; set; }

        public string ProvinceStateCode { get; set; }

        public string ProvinceName { get; set; }

        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long? AddressId { get; set; }

        public short RegionCode { get; set; }

        public short DistrictCode { get; set; }

        public string PropertyAreaUnitTypeCode { get; set; }

        public float? LandArea { get; set; }

        public string LandLegalDescription { get; set; }

        public string SurveyPlanNumber { get; set; }

        public string EncumbranceReason { get; set; }

        public bool IsSensitive { get; set; }

        public bool IsOwned { get; set; }

        public bool? IsRetired { get; set; }

        public bool IsVisibleToOtherAgencies { get; set; }

        public string Zoning { get; set; }

        public string ZoningPotential { get; set; }

        public bool? IsDisposed { get; set; }

        public bool? IsOtherInterest { get; set; }

        public bool? HasActiveAcquisitionFile { get; set; }

        public bool? HasActiveResearchFile { get; set; }

        public bool? IsPayableLease { get; set; }

        public bool? IsActivePayableLease { get; set; }

        public bool? IsReceivableLease { get; set; }

        public bool? IsActiveReceivableLease { get; set; }

        #endregion
    }
}
