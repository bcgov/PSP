namespace Pims.Api.Models.Concepts
{
    public class RegionUserModel : BaseAppModel
    {
        public long Id { get; set; }

        public UserModel User { get; set; }

        public long UserId { get; set; }

        public TypeModel<short> Region { get; set; }

        public short RegionCode { get; set; }
    }
}
