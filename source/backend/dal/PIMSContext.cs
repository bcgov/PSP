using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pims.Core.Extensions;
using Pims.Dal.Extensions;

namespace Pims.Dal
{
    /// <summary>
    /// PimsContext class, provides a data context to manage the datasource for the PIMS application.
    /// </summary>
    public class PimsContext : PimsBaseContext
    {
        #region Variables
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _serializerOptions;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PimsContext class.
        /// </summary>
        public PimsContext()
            : base()
        {
        }

        /// <summary>
        /// Creates a new instance of a PimsContext class.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="serializerOptions"></param>
        /// <returns></returns>
        public PimsContext(DbContextOptions<PimsContext> options, IHttpContextAccessor httpContextAccessor = null, IOptions<JsonSerializerOptions> serializerOptions = null)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _serializerOptions = serializerOptions.Value;
        }

        /// <summary>
        /// Save the entities with who created them or updated them.
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            // get entries that are being Added or Updated
            var modifiedEntries = ChangeTracker.Entries()
                    .Where(x => x.State.IsIn(EntityState.Added, EntityState.Modified, EntityState.Deleted));

            // Default values are provided because depending on the claim source it may or may not have these values.
            var username = _httpContextAccessor.HttpContext.User.GetUsername() ?? "service";
            var key = _httpContextAccessor.HttpContext.User.GetUserKey();
            var directory = _httpContextAccessor.HttpContext.User.GetUserDirectory() ?? string.Empty;
            foreach (var entry in modifiedEntries)
            {
                entry.UpdateAppAuditProperties(username, key, directory);
            }

            return base.SaveChanges();
        }

        /// <summary>
        /// Wrap the save changes in a transaction for rollback.
        /// </summary>
        /// <returns></returns>
        public int CommitTransaction()
        {
            var result = 0;
            using (var transaction = this.Database.BeginTransaction())
            {
                try
                {
                    result = this.SaveChanges();
                    transaction.Commit();
                }
                catch (DbUpdateException)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// Deserialize the specified 'json' to the specified type of 'T'.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the object to.</typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _serializerOptions);
        }

        /// <summary>
        /// Serialize the specified 'item'.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public string Serialize<T>(T item)
        {
            return JsonSerializer.Serialize(item, _serializerOptions);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Configures the DbContext with the specified options.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.EnableSensitiveDataLogging();
            }

            base.OnConfiguring(optionsBuilder);
        }
        #endregion
    }
}
