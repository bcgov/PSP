
#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsxTableDefinition
    {
        public string TableName { get; set; }
        public string TableAlias { get; set; }
        public string HistRequired { get; set; }
        public string Description { get; set; }
    }
}
