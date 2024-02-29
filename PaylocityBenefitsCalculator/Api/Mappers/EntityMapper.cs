using Api.Dtos;
using Api.Models;

namespace Api.Mappers;

public interface IEntityMapper<TEntity, TDto>
    where TEntity : IEntity
    where TDto : IDto
{
    TEntity Map(TDto dto);
    IEnumerable<TEntity> Map(IEnumerable<TDto> dtos);
}

public abstract class EntityMapperBase<TEntity, TDto> : IEntityMapper<TEntity, TDto>
    where TEntity : IEntity
    where TDto : IDto
{
    public abstract TEntity Map(TDto dto);

    public IEnumerable<TEntity> Map(IEnumerable<TDto> dtos)
    {
        foreach (var dto in dtos)
            yield return Map(dto);
    }
}
