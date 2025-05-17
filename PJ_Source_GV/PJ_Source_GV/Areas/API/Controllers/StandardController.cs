using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PJ_Source_GV.Models.Models.Dtos;
using PJ_Source_GV.Repositories;

namespace PJ_Source_GV.Areas.API.Controllers;

public class StandardController : Controller
{
    private readonly IStandardRepository _standardRepository;
    
    
    public StandardController(IStandardRepository standardRepository)
    {
        _standardRepository = standardRepository;
    }
    [Route("API/Standard")]
    [Area("API")]
    public async Task<IActionResult> GetAllGroup()
    {
        var items = await _standardRepository.GetAll();
        if (items.Any())
        {
            return Ok(items);
        }

        return NoContent();
    }
    
    [Route("API/Standard")]
    [Area("API")]
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
}   