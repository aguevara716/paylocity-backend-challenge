using Api.Models;

namespace Api.Repositories;

public interface IRepository<T>
	where T : IEntity
{
	// create
	T Create(T newEntity);
	IEnumerable<T> Create(IEnumerable<T> newEntities);

	// read
	IQueryable<T> Get();
	T? Get(int id);

	// update

	// delete
}
