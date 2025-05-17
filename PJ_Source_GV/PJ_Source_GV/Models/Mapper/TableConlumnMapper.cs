using PJ_Source_GV.Models.Entities;
using PJ_Source_GV.Models.Models.Dtos;

namespace PJ_Source_GV.Models.Mapper;

public class TableConlumnMapper
{
    public static TableColumnDto ToDto(TableColumnEntity entity)
    {
        return new TableColumnDto
        {
            Id = entity.id,
            TableId = entity.table_id,
            Name = entity.name,
            NormalizedName = entity.normalized_name,
            Order = entity.order,
            CreatedAt = entity.created_at,
            UpdatedAt = entity.updated_at,
            CreatedBy = entity.created_by,
            UpdatedBy = entity.updated_by
        };
    }
    
    public static TableColumnEntity ToEntity(TableColumnDto dto)
    {
        return new TableColumnEntity
        {
            id = dto.Id,
            table_id = dto.TableId,
            name = dto.Name,
            normalized_name = dto.NormalizedName,
            order = dto.Order,
            created_at = dto.CreatedAt,
            updated_at = dto.UpdatedAt,
            created_by = dto.CreatedBy,
            updated_by = dto.UpdatedBy
        };
    }
}