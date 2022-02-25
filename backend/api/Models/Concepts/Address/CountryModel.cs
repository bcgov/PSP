
namespace Pims.Api.Models.Concepts
{
    public class CountryModel : BaseAppModel
    {
        public short CountryId { get; set; }
        public string CountryCode { get; set; }
        public string Description { get; set; }
    }
}