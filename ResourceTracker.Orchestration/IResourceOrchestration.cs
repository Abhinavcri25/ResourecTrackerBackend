using ResourceTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTracker.Orchestration
{
    public interface IResourceOrchestration
    {
        Task<List<Resource>> GetAll();
        Task<Resource> Get(int id);
        Task<bool> Add(Resource resource);
        Task<bool> Update(Resource resource);
        Task<bool> Delete(int id);
        //Task<Resource> GetByEmail(string email);

        Task<List<string>> GetAllSkillsAsync();
        Task<List<string>> GetAllLocationsAsync();
        Task<List<string>> GetAllDesignationsAsync();

    }
}
