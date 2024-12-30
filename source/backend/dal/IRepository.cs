using Microsoft.EntityFrameworkCore.Storage;

namespace Pims.Dal
{
    public interface IRepository
    {
        #region Methods

        IDbContextTransaction BeginTransaction();

        void SaveChanges();

        void CommitTransaction();
        #endregion
    }
}
