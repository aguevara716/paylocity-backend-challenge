using Api.Dtos;
using Api.Models;

namespace Api.Mappers;

public interface IDtoMapper<TDto, TEntity>
    where TDto : IDto
    where TEntity : IEntity
{
    TDto Map(TEntity entity);
    IEnumerable<TDto> Map(IEnumerable<TEntity> entities);
}

public abstract class DtoMapperBase<TDto, TEntity> : IDtoMapper<TDto, TEntity>
    where TDto : IDto
    where TEntity : IEntity
{
    public abstract TDto Map(TEntity entity);

    public IEnumerable<TDto> Map(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            yield return Map(entity);
    }
}

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
