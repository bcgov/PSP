namespace Pims.Dal.Repositories
{
    public interface ISequenceRepository
    {
        long GetNextSequenceValue(string sequenceName);
    }
}
