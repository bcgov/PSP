namespace Pims.Api.Models
{
    /// <summary>
    /// Systen constant class, provides a model that represents a system constant.
    /// </summary>
    public class SystemConstantModel
    {
        #region Properties

        /// <summary>
        /// get/set - The constant name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - The value of the constant.
        /// </summary>
        public string Value { get; set; }
        #endregion
    }
}
