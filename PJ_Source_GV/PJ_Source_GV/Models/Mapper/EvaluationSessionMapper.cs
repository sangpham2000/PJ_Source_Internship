// PJ_Source_GV.Models.Mapper/EvaluationSessionMapper.cs
using PJ_Source_GV.Models.Entities;
using PJ_Source_GV.Models.Models.Dtos;

namespace PJ_Source_GV.Models.Mapper;

public class EvaluationSessionMapper
{
    public static EvaluationSessionDto ToDto(EvaluationSessionEntity entity)
    {
        return new EvaluationSessionDto
        {
            Id = entity.id,
            EvaluationId = entity.evaluation_id,
            Desc = entity.desc,
            CreatedAt = entity.created_at,
            UpdatedAt = entity.updated_at,
            CreatedBy = entity.created_by,
            UpdatedBy = entity.updated_by,
            Status = entity.status
        };
    }

    public static EvaluationSessionEntity ToEntity(EvaluationSessionDto dto)
    {
        return new EvaluationSessionEntity
        {
            id = dto.Id,
            evaluation_id = dto.EvaluationId,
            desc = dto.Desc,
            created_at = dto.CreatedAt,
            updated_at = dto.UpdatedAt,
            created_by = dto.CreatedBy,
            updated_by = dto.UpdatedBy,
            status = dto.Status
        };
    }
}