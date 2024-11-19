using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pims.Dal;
using Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// PimsTestContext class, provides a data context to manage the datasource for the PIMS application (specific to UNIT TESTS).
    /// </summary>
    public class PimsTestContext : PimsContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PimsTestContext"/> class.
        /// </summary>
        public PimsTestContext()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PimsTestContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="httpContextAccessor">Provides access to the current IHttpContextAccessor.HttpContext, if one is available.</param>
        /// <param name="serializerOptions">The serializer options.</param>
        public PimsTestContext(DbContextOptions<PimsContext> options, IHttpContextAccessor httpContextAccessor = null, IOptions<JsonSerializerOptions> serializerOptions = null)
            : base(options, httpContextAccessor, serializerOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // This is needed so unit tests for DB Views work with in-memory DB
            modelBuilder.Entity<PimsPropertyVw>(entity =>
            {
                entity.HasKey(x => x.PropertyId);
            });
        }
    }
}
