namespace Pims.Api.Models.Concepts.Property
{
    /**
    * Code type that contains the parent type code as well
    **/
    public class PropertyActivitySubtypeModel
    {
        #region Properties

        public string TypeCode { get; set; }

        public string ParentTypeCode { get; set; }

        public string Description { get; set; }

        public bool? IsDisabled { get; set; }

        #endregion
    }
}
