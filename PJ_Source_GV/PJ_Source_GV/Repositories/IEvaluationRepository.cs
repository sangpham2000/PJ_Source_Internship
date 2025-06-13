using System.Collections.Generic;
using System.Threading.Tasks;
using PJ_Source_GV.Models;
using PJ_Source_GV.Models.Models.Dtos;
using PJ_Source_GV.Models.Vocabulary;

namespace PJ_Source_GV.Repositories
{
    public interface IEvaluationRepository
    {
        Task<List<EvaluationDto>> GetAll();
        Task<PaginatedResult<EvaluationDto>> GetAllPaginated(EvaluationPaginationRequest request);
        Task<EvaluationDto?> GetById(int id);
        Task<List<EvaluationSessionDto>> GetSessionsByEvaluationId(int evaluationId);
        Task<int> Add(EvaluationDto evaluationDto);
        Task<int> AddSession(EvaluationSessionDto sessionDto);
        Task<int> AddEvaluationValueAsync(EvaluationSessionDto sessionDto, SessionStatus mode);
        Task<int> Update(EvaluationDto evaluationDto);
        Task<bool> Delete(int id);
        Task SaveEvaluationHistory(int evaluationId, int userId);
        Task<List<EvaluationHistoryDto>> GetEvaluationHistory(int evaluationId);
    }
}