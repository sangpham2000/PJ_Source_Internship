using System;

namespace PJ_Source_GV.Models.Models.Dtos;

public class TableColumnDto
{
    public int? Id { get; set; }
    public int? TableId { get; set; }
    public string Name { get; set; }
    public int? Type { get; set; } = 0;
    public int? Order { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
}