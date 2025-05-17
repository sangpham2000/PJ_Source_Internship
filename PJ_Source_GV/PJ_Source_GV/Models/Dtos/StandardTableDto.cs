using System;

namespace PJ_Source_GV.Models.Models.Dtos;

public class StandardTableDto
{
    public int Id { get; set; }
    public int StandardId { get; set; }
    public string Name { get; set; }
    public string NormalizedName { get; set; }
    public int Order { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int CreatedBy { get; set; }
    public int UpdatedBy { get; set; }
}