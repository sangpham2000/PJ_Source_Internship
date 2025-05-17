using System;

namespace PJ_Source_GV.Models.Entities;

public class ColumnCellEntity
{
    public int id { get; set; }
    public int column_id { get; set; }
    public string value { get; set; }
    public int order { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public int created_by { get; set; }
    public int updated_by { get; set; }
}