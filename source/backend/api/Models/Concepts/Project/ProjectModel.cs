namespace Pims.Api.Models.Concepts
{
    public class ProjectModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - The project id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The status type code.
        /// </summary>
        public TypeModel<string> ProjectStatusTypeCode { get; set; }

        /// <summary>
        /// get/set - Bussiness function code.
        /// </summary>
        public long? BusinessFunctionCode { get; set; } // TODO: Use the correct type.

        /// <summary>
        /// get/set - Cost type code.
        /// </summary>
        public long? CostTypeCode { get; set; } // TODO: Use the correct type.

        /// <summary>
        /// get/set - Work activity code.
        /// </summary>
        public long? WorkActivityCode { get; set; } // TODO: Use the correct type.

        /// <summary>
        /// get/set - The region code.
        /// </summary>
        public CodeTypeModel RegionCode { get; set; } // TODO: Use the correct type.

        /// <summary>
        /// get/set - The project code.
        /// </summary>
        public int? Code { get; set; }

        /// <summary>
        /// get/set - Project description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - Project notes.
        /// </summary>
        public string Note { get; set; }
        #endregion
    }
}
