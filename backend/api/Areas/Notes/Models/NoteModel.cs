using System;

namespace Pims.Api.Areas.Notes.Models
{
    public class NoteModel
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public DateTime AppCreateTimestamp { get; set; }
        public string AppLastUpdateUserid { get; set; }
    }
}
