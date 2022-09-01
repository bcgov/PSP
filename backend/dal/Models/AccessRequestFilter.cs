namespace Pims.Dal.Entities.Models
{
    /// <summary>
    /// AccessRequestFilter class, provides a way to search access requests.
    /// </summary>
    public class AccessRequestFilter : PageFilter
    {
        /// <summary>
        /// Creates a new instance of an AccessRequestFilter object, initializes with specified arguments.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <param name="searchText"></param>
        /// <param name="sort"></param>
        public AccessRequestFilter(int page = 1, int quantity = 10, string searchText = "", string[] sort = null)
            : base(page, quantity, sort)
        {
            this.SearchText = searchText;
        }

        /// <summary>
        /// get/set - The access request page uses a single search field that searches against the last name and IDIR (business identifier).
        /// </summary>
        public string SearchText { get; set; }

        public PimsAccessRequestStatusType StatusType { get; set; }
    }
}
