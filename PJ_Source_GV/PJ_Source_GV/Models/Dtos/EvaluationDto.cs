using System;
using System.Collections.Generic;

namespace PJ_Source_GV.Models.Models.Dtos;

public class EvaluationDto
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
    public List<EvaluationSessionDto> Sessions { get; set; } = new List<EvaluationSessionDto>();
}