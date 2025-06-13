#nullable enable
using System;
using System.Collections.Generic;

namespace PJ_Source_GV.Models.Models.Dtos;

public class StandardDto
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public string? NormalizedName { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public int? UpdatedBy { get; set; }
    public int? TotalCount { get; set; }
    public List<StandardTableDto?> Tables { get; set; } = new List<StandardTableDto?>();
}



