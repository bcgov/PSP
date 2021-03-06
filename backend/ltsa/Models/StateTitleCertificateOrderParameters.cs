/* 
 * Title Direct Search Services
 *
 * Title Direct Search Services
 *
 * OpenAPI spec version: 4.0.1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System.IO;
using System.Runtime.Serialization;


namespace Pims.Ltsa.Models
{
    /// <summary>
    /// Parameteres required for ordering a State Title Certificate
    /// </summary>
    [DataContract]
    public partial class StateTitleCertificateOrderParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StateTitleCertificateOrderParameters" /> class.
        /// </summary>
        /// <param name="titleNumber">Title number of a registered or pending title (required).</param>
        /// <param name="pendingApplicationNumber">Optional application number of a pending application for the title.</param>
        /// <param name="ltoClientNumber">LTO client number One of LTO client number or recipient name/address must be provided # Note: If the LTO client number is provided, the recipient name/address will be ignored if also provided. .</param>
        /// <param name="recipientName">Name of the recipient to be printed on the STC.</param>
        /// <param name="landTitleDistrictCode">landTitleDistrictCode.</param>
        /// <param name="recipientAddress">recipientAddress.</param>
        public StateTitleCertificateOrderParameters(string titleNumber = default, string pendingApplicationNumber = default, string ltoClientNumber = default, string recipientName = default, LandTitleDistrictCode landTitleDistrictCode = default, RecipientAddress recipientAddress = default)
        {
            // to ensure "titleNumber" is required (not null)
            if (titleNumber == null)
            {
                throw new InvalidDataException("titleNumber is a required property for StateTitleCertificateOrderParameters and cannot be null");
            }
            else
            {
                this.TitleNumber = titleNumber;
            }
            this.PendingApplicationNumber = pendingApplicationNumber;
            this.LtoClientNumber = ltoClientNumber;
            this.RecipientName = recipientName;
            this.LandTitleDistrictCode = landTitleDistrictCode;
            this.RecipientAddress = recipientAddress;
        }

        /// <summary>
        /// Title number of a registered or pending title
        /// </summary>
        /// <value>Title number of a registered or pending title</value>
        [DataMember(Name = "titleNumber", EmitDefaultValue = false)]
        public string TitleNumber { get; set; }

        /// <summary>
        /// Optional application number of a pending application for the title
        /// </summary>
        /// <value>Optional application number of a pending application for the title</value>
        [DataMember(Name = "pendingApplicationNumber", EmitDefaultValue = false)]
        public string PendingApplicationNumber { get; set; }

        /// <summary>
        /// LTO client number One of LTO client number or recipient name/address must be provided # Note: If the LTO client number is provided, the recipient name/address will be ignored if also provided. 
        /// </summary>
        /// <value>LTO client number One of LTO client number or recipient name/address must be provided # Note: If the LTO client number is provided, the recipient name/address will be ignored if also provided. </value>
        [DataMember(Name = "ltoClientNumber", EmitDefaultValue = false)]
        public string LtoClientNumber { get; set; }

        /// <summary>
        /// Name of the recipient to be printed on the STC
        /// </summary>
        /// <value>Name of the recipient to be printed on the STC</value>
        [DataMember(Name = "recipientName", EmitDefaultValue = false)]
        public string RecipientName { get; set; }

        /// <summary>
        /// Gets or Sets LandTitleDistrictCode
        /// </summary>
        [DataMember(Name = "landTitleDistrictCode", EmitDefaultValue = false)]
        public LandTitleDistrictCode LandTitleDistrictCode { get; set; }

        /// <summary>
        /// Gets or Sets RecipientAddress
        /// </summary>
        [DataMember(Name = "recipientAddress", EmitDefaultValue = false)]
        public RecipientAddress RecipientAddress { get; set; }
    }
}
