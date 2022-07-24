namespace CSBlog.Core.Repository;

public interface IRepository<T> where T : class
{
  ICollection<T> GetAll();
  T GetById(string? id);
  T GetByName(string? name);
  Task Create(T item);
  Task Update(T item);
  Task Delete(T item);
}