namespace Pims.Api.Models.Concepts.Property
{
    /// <summary>
    /// Represents the result of a PID lookup against PIMS and PMBC.
    /// </summary>
    public class PropertyPidLookupResultModel
    {
        /// <summary>
        /// True when a matching record exists in PIMS.
        /// </summary>
        public bool FoundInPims { get; set; }

        /// <summary>
        /// The property result. For PMBC records this is projected into PropertyModel shape.
        /// </summary>
        public PropertyModel Property { get; set; }
    }
}
