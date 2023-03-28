using Pims.Api.Models.Lookup;

namespace Pims.Api.Models.Concepts
{
    public class FileFormModel : BaseAppModel
    {
        public long Id { get; set; }

        public long FileId { get; set; }

        public LookupModel<string> FormTypeCode { get; set; }
    }
}
