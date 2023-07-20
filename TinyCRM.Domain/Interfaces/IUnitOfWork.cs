namespace TinyCRM.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangeAsync();
    }
}