/* 
 * Title Direct Search Services
 *
 * Title Direct Search Services
 *
 * OpenAPI spec version: 4.0.1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pims.Ltsa.Models
{
    /// <summary>
    /// DocOrPlanSummary
    /// </summary>
    [DataContract]
    public partial class DocOrPlanSummary
    {
        /// <summary>
        /// Status of the document or plan. Note: Only document or plan with the status “Found” can be ordered via this interface.
        /// </summary>
        /// <value>Status of the document or plan. Note: Only document or plan with the status “Found” can be ordered via this interface.</value>
        [JsonStringEnumMemberConverterOptions(deserializationFailureFallbackValue: 0)]
        [JsonConverter(typeof(JsonStringEnumMemberConverter))]
        public enum StatusEnum
        {
            Unknown = 0,
            /// <summary>
            /// Enum Found for value: Found.
            /// </summary>
            [EnumMember(Value = "Found.")]
            Found = 1,
            /// <summary>
            /// Enum ObtainatLandTitleOffice for value: Obtain at Land Title Office.
            /// </summary>
            [EnumMember(Value = "Obtain at Land Title Office.")]
            ObtainatLandTitleOffice = 2,
            /// <summary>
            /// Enum Planwillbescanned for value: Plan will be scanned.
            /// </summary>
            [EnumMember(Value = "Plan will be scanned.")]
            Planwillbescanned = 3,
            /// <summary>
            /// Enum Documentwillbescanned for value: Document will be scanned.
            /// </summary>
            [EnumMember(Value = "Document will be scanned.")]
            Documentwillbescanned = 4
        }
        /// <summary>
        /// Status of the document or plan. Note: Only document or plan with the status “Found” can be ordered via this interface.
        /// </summary>
        /// <value>Status of the document or plan. Note: Only document or plan with the status “Found” can be ordered via this interface.</value>
        [DataMember(Name = "status", EmitDefaultValue = false)]
        public StatusEnum? Status { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="DocOrPlanSummary" /> class.
        /// </summary>
        /// <param name="docOrPlanNumber">Document or plan number.</param>
        /// <param name="documentDistrict">Document district (e.g. Victoria) represents the Land Title Office that received the document for processing..</param>
        /// <param name="documentDistrictCode">documentDistrictCode.</param>
        /// <param name="status">Status of the document or plan. Note: Only document or plan with the status “Found” can be ordered via this interface..</param>
        public DocOrPlanSummary(string docOrPlanNumber = default, string documentDistrict = default, LandTitleDistrictCode documentDistrictCode = default, StatusEnum? status = default)
        {
            this.DocOrPlanNumber = docOrPlanNumber;
            this.DocumentDistrict = documentDistrict;
            this.DocumentDistrictCode = documentDistrictCode;
            this.Status = status;
        }

        /// <summary>
        /// Document or plan number
        /// </summary>
        /// <value>Document or plan number</value>
        [DataMember(Name = "docOrPlanNumber", EmitDefaultValue = false)]
        public string DocOrPlanNumber { get; set; }

        /// <summary>
        /// Document district (e.g. Victoria) represents the Land Title Office that received the document for processing.
        /// </summary>
        /// <value>Document district (e.g. Victoria) represents the Land Title Office that received the document for processing.</value>
        [DataMember(Name = "documentDistrict", EmitDefaultValue = false)]
        public string DocumentDistrict { get; set; }

        /// <summary>
        /// Gets or Sets DocumentDistrictCode
        /// </summary>
        [DataMember(Name = "documentDistrictCode", EmitDefaultValue = false)]
        public LandTitleDistrictCode DocumentDistrictCode { get; set; }
    }
}
