using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PJ_Source_GV.Models.Models.Dtos;
using PJ_Source_GV.Models.Vocabulary;
using PJ_Source_GV.Repositories;

namespace PJ_Source_GV.Areas.API.Controllers
{
    [Route("API/Evaluation")]
    [Area("API")]
    [ApiController]
    public class EvaluationController : ControllerBase
    {
        private readonly IEvaluationRepository _evaluationRepository;

        public EvaluationController(IEvaluationRepository evaluationRepository)
        {
            _evaluationRepository = evaluationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _evaluationRepository.GetAll();
            if (items.Count != 0)
            {
                return Ok(items);
            }
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _evaluationRepository.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> AddEvaluation([FromBody] EvaluationDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _evaluationRepository.Add(request);
            if (result == 0)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetById), new { id = result }, request);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEvaluation([FromBody] EvaluationDto request)
        {
            if (!ModelState.IsValid || request.Id == null)
            {
                return BadRequest(ModelState);
            }

            var result = await _evaluationRepository.Update(request);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok(request);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvaluation(int id)
        {
            var result = await _evaluationRepository.Delete(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        
        [HttpGet("session/{evaluationId}")]
        public async Task<IActionResult> GetSessionsByEvaluationId(int evaluationId)
        {
            var item = await _evaluationRepository.GetSessionsByEvaluationId(evaluationId);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        
        [HttpPost("session")]
        public async Task<IActionResult> AddSession([FromBody] EvaluationSessionDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _evaluationRepository.AddSession(request);
            if (result == 0)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetById), new { id = result }, request);
        }
        [HttpPost("add-session-value/{mode}")]
        public async Task<IActionResult> AddSessionValue([FromBody] EvaluationSessionDto request, [FromRoute] SessionStatus mode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _evaluationRepository.AddEvaluationValueAsync(request, mode);
            if (result == 0)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetById), new { id = result }, request);
        }

        [HttpPost("{id}/history")]
        public async Task<IActionResult> SaveEvaluationHistory(int id, [FromBody] int userId)
        {
            try
            {
                await _evaluationRepository.SaveEvaluationHistory(id, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}/history")]
        public async Task<IActionResult> GetEvaluationHistory(int id)
        {
            var history = await _evaluationRepository.GetEvaluationHistory(id);
            if (history.Count == 0)
            {
                return NoContent();
            }
            return Ok(history);
        }
    }
}