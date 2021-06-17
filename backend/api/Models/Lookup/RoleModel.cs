namespace Pims.Api.Models.Lookup
{
    public class RoleModel : CommonLookupModel<long>
    {
        #region Properties
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        #endregion
    }
}
