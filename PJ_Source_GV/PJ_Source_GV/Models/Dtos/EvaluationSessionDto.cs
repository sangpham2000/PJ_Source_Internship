using System;
using System.Collections.Generic;
using PJ_Source_GV.Models.Vocabulary;

namespace PJ_Source_GV.Models.Models.Dtos;

public class EvaluationSessionDto
{
    public int? Id { get; set; }
    public int? EvaluationId { get; set; }
    public string? Desc { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
    public SessionStatus? Status { get; set; }
    public List<int> StandardIds { get; set; } = new List<int>();
    public List<string> StandardNames { get; set; } = new List<string>();
    public List<ColumnCellDto> Cells { get; set; } = new List<ColumnCellDto>();
}