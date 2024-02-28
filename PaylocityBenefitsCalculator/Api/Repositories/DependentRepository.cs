using Api.Models;

namespace Api.Repositories;

public interface IDependentRepository : IRepository<Dependent>
{

}

public sealed class DependentRepository : IDependentRepository
{
    private readonly BenefitsDbContext _context;

    public DependentRepository(BenefitsDbContext context)
    {
        _context = context;
    }

    // create
    public Dependent Create(Dependent newDependent)
    {
        var nextId = _context.Dependents.Max(d => d.Id) + 1;
        newDependent.Id = nextId;

        _context.Dependents.Add(newDependent);
        return newDependent;
    }

    public IEnumerable<Dependent> Create(IEnumerable<Dependent> newDependents)
    {
        foreach (var newDependent in newDependents)
            yield return Create(newDependent);
    }

    // read
    public IQueryable<Dependent> Get()
    {
        return _context.Dependents.AsQueryable();
    }

    public Dependent? Get(int id)
    {
        return _context.Dependents.FirstOrDefault(d => d.Id == id);
    }

    // update

    // delete
}
