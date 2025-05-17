using System;

namespace PJ_Source_GV.Models.Models.Dtos;

public class TableCellDto
{
    public int Id { get; set; }
    public int TableId { get; set; }
    public string Value { get; set; }
    public int Order { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
}