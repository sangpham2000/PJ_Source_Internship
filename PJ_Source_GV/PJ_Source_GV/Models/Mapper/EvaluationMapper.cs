// PJ_Source_GV.Models.Mapper/EvaluationMapper.cs
using PJ_Source_GV.Models.Entities;
using PJ_Source_GV.Models.Models.Dtos;

namespace PJ_Source_GV.Models.Mapper;

public class EvaluationMapper
{
    public static EvaluationDto ToDto(EvaluationEntity entity)
    {
        return new EvaluationDto
        {
            Id = entity.id,
            Name = entity.name,
            Desc = entity.desc,
            StartDate = entity.start_date,
            EndDate = entity.end_date,
            CreatedAt = entity.created_at,
            UpdatedAt = entity.updated_at,
            CreatedBy = entity.created_by,
            UpdatedBy = entity.updated_by
        };
    }

    public static EvaluationEntity ToEntity(EvaluationDto dto)
    {
        return new EvaluationEntity
        {
            id = dto.Id,
            name = dto.Name,
            desc = dto.Desc,
            start_date = dto.StartDate,
            end_date = dto.EndDate,
            created_at = dto.CreatedAt,
            updated_at = dto.UpdatedAt,
            created_by = dto.CreatedBy,
            updated_by = dto.UpdatedBy
        };
    }
}