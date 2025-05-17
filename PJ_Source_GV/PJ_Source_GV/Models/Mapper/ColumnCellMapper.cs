using PJ_Source_GV.Models.Entities;
using PJ_Source_GV.Models.Models.Dtos;

namespace PJ_Source_GV.Models.Mapper;

public class ColumnCellMapper
{
    public static ColumnCellDto ToDto(ColumnCellEntity entity)
    {
        return new ColumnCellDto
        {
            Id = entity.id,
            ColumnId = entity.column_id,
            Value = entity.value,
            CreatedAt = entity.created_at,
            UpdatedAt = entity.updated_at,
            CreatedBy = entity.created_by,
            UpdatedBy = entity.updated_by
        };
    } 
    
    public static ColumnCellEntity ToEntity(ColumnCellDto dto)
    {
        return new ColumnCellEntity
        {
            id = dto.Id,
            column_id = dto.ColumnId,
            value = dto.Value,
            created_at = dto.CreatedAt,
            updated_at = dto.UpdatedAt,
            created_by = dto.CreatedBy,
            updated_by = dto.UpdatedBy
        };
    } 
}