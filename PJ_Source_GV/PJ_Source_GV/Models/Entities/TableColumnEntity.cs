using System;

namespace PJ_Source_GV.Models.Entities;

public class TableColumnEntity
{
    public int? id { get; set; }
    public int? table_id { get; set; }
    public string name { get; set; }
    public int? type { get; set; }
    public int? order { get; set; }
    public DateTime? created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public int? created_by { get; set; }
    public int? updated_by { get; set; }
}