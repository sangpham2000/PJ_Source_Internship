using PJ_Source_GV.Models.Entities;
using PJ_Source_GV.Models.Models.Dtos;

namespace PJ_Source_GV.Models.Mapper;

public static class StandardTableMapper
{
    public static StandardTableDto ToDto(StandardTableEntity entity)
    {
        return new StandardTableDto
        {
            Id = entity.id,
            StandardId = entity.standard_id,
            Name = entity.name,
            Order = entity.order,
            CreatedAt = entity.created_at,
            UpdatedAt = entity.updated_at,
            CreatedBy = entity.created_by,
            UpdatedBy = entity.updated_by
        };
    }

    public static StandardTableEntity ToEntity(StandardTableDto dto)
    {
        return new StandardTableEntity
        {
            id = dto.Id,
            standard_id = dto.StandardId,
            name = dto.Name,
            order = dto.Order,
            created_at = dto.CreatedAt,
            updated_at = dto.UpdatedAt,
            created_by = dto.CreatedBy,
            updated_by = dto.UpdatedBy
        };
    }
}