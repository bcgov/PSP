
namespace Pims.Dal.Repositories
{
    public interface IEntityNoteRepository : IRepository
    {
        T Add<T>(T entity) where T : class;
    }
}
