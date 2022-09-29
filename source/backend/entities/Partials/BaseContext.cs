using Microsoft.EntityFrameworkCore;

namespace Pims.Dal
{
    /// <summary>
    /// Partial PimsBaseContext class that provides a protected constructor that allows for inheritance with a dynamic DbContextOptions object.
    /// </summary>
    public partial class PimsBaseContext : DbContext
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="PimsBaseContext"/> class.
        /// Protected to remove it from the the dependency injection service.
        /// </summary>
        /// <param name="options">The DBContextOptions of any generic type.</param>
        protected PimsBaseContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
