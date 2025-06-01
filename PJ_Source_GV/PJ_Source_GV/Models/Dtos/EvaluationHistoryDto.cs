using System;

namespace PJ_Source_GV.Models.Models.Dtos;

public class EvaluationHistoryDto
{
    public int EvaluationId { get; set; }
    public string EvaluationName { get; set; }
    public string StandardName { get; set; }
    public string TableName { get; set; }
    public string ColumnName { get; set; }
    public string CellValue { get; set; }
    public DateTime? EvaluationTime { get; set; }
}