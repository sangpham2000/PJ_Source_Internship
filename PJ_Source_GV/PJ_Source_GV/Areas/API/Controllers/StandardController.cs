using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PJ_Source_GV.Models;
using PJ_Source_GV.Models.Models.Dtos;
using PJ_Source_GV.Repositories;

namespace PJ_Source_GV.Areas.API.Controllers;

[Route("API/Standard")]
[Area("API")]
[ApiController]
public class StandardController : Controller
{
    private readonly IStandardRepository _standardRepository;
    
    
    public StandardController(IStandardRepository standardRepository)
    {
        _standardRepository = standardRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllPaginated([FromQuery] PaginationRequest request)
    {
        // Validate pagination parameters
        if (request.Page < 1)
            request.Page = 1;
    
        if (request.PageSize < 1 || request.PageSize > 100)
            request.PageSize = 10;

        var result = await _standardRepository.GetAllPaginated(request);
    
        if (result.Items.Any())
        {
            return Ok(result);
        }

        return NoContent();
    }
    
    [HttpPost]
    public async Task<IActionResult> AddStandard([FromBody] StandardDto request)
    {
        var result = await _standardRepository.Add(request);
        if (result == 0)
        {
            return BadRequest();
        }
        return Created();
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateStandard([FromBody] StandardDto request)
    {
        var result = await _standardRepository.Update(request);
        if (result == 0)
        {
            return BadRequest();
        }
        return Ok(request);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStandard([FromRoute] int id)
    {
        var result = await _standardRepository.Delete(id);
        return NoContent();
    }
    
    [HttpPost("GetByIds")]
    [HttpPost("GetByIds/{evaluationSessionId}")]
    public async Task<IActionResult> GetByIds([FromBody] List<int> ids, [FromRoute] int? evaluationSessionId = null)
    {
        if (ids == null || !ids.Any())
        {
            return BadRequest(new { Message = "List of IDs cannot be null or empty." });
        }

        List<StandardDto> items; // Initialize the list to hold the data
        if (evaluationSessionId != null)
        {
            items = await _standardRepository.GetHistoryByEvaluationSessionId(ids, (int)evaluationSessionId);
        }
        else
        {
            items = await _standardRepository.GetByIds(ids);
        }
        if (items.Count == 0)
        {
            return NoContent();
        }
        return Ok(items);
    }
}   