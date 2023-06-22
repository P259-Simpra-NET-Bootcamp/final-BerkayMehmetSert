namespace Core.Persistence.Repositories;

public interface IUnitOfWork : IDisposable
{
    void SaveChanges();
}