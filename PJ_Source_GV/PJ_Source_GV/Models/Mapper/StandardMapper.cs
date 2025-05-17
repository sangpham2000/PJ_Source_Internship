using PJ_Source_GV.Models.Entities;
using PJ_Source_GV.Models.Models.Dtos;

namespace PJ_Source_GV.Models.Mapper;

public static class StandardMapper
{
    public static StandardDto ToDto(StandardEntity entity)
    {
        return new StandardDto
        {
            Id = entity.id,
            Name = entity.name,
            NormalizedName = entity.normalized_name,
            CreatedAt = entity.created_at,
            UpdatedAt = entity.updated_at,
            CreatedBy = entity.created_by,
            UpdatedBy = entity.updated_by
        };
    }

    public static StandardEntity ToEntity(StandardDto dto)
    {
        return new StandardEntity
        {
            id = dto.Id,
            name = dto.Name,
            normalized_name = dto.NormalizedName,
            created_at = dto.CreatedAt,
            updated_at = dto.UpdatedAt,
            created_by = dto.CreatedBy,
            updated_by = dto.UpdatedBy
        };
    }
}