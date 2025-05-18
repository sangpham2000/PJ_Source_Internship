using System.Collections.Generic;
using System.Threading.Tasks;
using PJ_Source_GV.Models.Models.Dtos;

namespace PJ_Source_GV.Repositories;

public interface IStandardRepository
{
    Task<List<StandardDto>> GetAll();
    Task<StandardDto?> GetById(int id);
    Task<int> Add(StandardDto standard);
    Task<int> Update(StandardDto standard);
    Task<bool>  Delete(int id);
}