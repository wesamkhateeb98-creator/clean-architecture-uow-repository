namespace Shop.Domain.Entities;

// Base class for every entity: lets IRepository<T> work with any entity by Id.
public class IEntity
{
    public int Id { get; set; }
}
