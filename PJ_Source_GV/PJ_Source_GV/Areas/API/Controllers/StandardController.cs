using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> GetAllGroup()
    {
        var items = await _standardRepository.GetAll();
        if (items.Any())
        {
            return Ok(items);
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
}   