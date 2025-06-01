using System;

namespace PJ_Source_GV.Models.Entities;

public class EvaluationEntity
{
    public int? id { get; set; }
    public string name { get; set; }
    public string desc { get; set; }
    public DateTime? start_date { get; set; }
    public DateTime? end_date { get; set; }
    public DateTime? created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public int? created_by { get; set; }
    public int? updated_by { get; set; }
}