using System;
using PJ_Source_GV.Models.Vocabulary;

namespace PJ_Source_GV.Models.Entities;

public class EvaluationSessionEntity
{
    public int? id { get; set; }
    public int? evaluation_id { get; set; }
    public string? desc { get; set; }
    public DateTime? created_at { get; set; }
    public DateTime? updated_at { get; set; }
    public int? created_by { get; set; }
    public int? updated_by { get; set; }
    public SessionStatus? status { get; set; }
}

