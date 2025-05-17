using System.Collections.Generic;
using System.Threading.Tasks;
using PJ_Source_GV.Models.Models.Dtos;

namespace PJ_Source_GV.Repositories;

public interface IStandardRepository
{
    Task<List<StandardDto>> GetAll();
    StandardDto GetById(string id);
    Task<int> Add(StandardDto standard);
    void Update(StandardDto standard);
    void Delete(string id);
}