using ResourceTracker.DAO;
using ResourceTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ResourceTracker.Orchestration
{
    public class ResourceOrchestration : IResourceOrchestration
    {
        private readonly IResourceDAO _repo;

        public ResourceOrchestration(IResourceDAO repo) { _repo = repo; }

        //public Task<List<Resource>> GetAll() => _repo.GetAll();
        public async Task<List<Resource>> GetAll()
        {
            return await Task.Run(() => _repo.GetAll());
        }

        //public Task<Resource> Get(int id) => _repo.Get(id);
        public Task<Resource> Get(int id)
        {
            return Task.Run(() => _repo.Get(id));
        }

        public Task<bool> Add(Resource resource)
        {
            return Task.Run(() => _repo.Add(resource));
        }
        //public Task<bool> Update(Resource resource) => _repo.update(resource);
        public Task<bool> Update(Resource resource) 
        {
            return Task.Run(() => _repo.update(resource));
        }
        //public Task<bool> Delete(int id) => _repo.delete(id);
        public Task<bool> Delete(int id)
        {
            return Task.Run(() => _repo.delete(id));
        }

        public Task<List<string>> GetAllSkillsAsync()
        {
            return Task.Run(() => _repo.GetAllSkillsAsync());
        }

        public Task<List<string>> GetAllLocationsAsync()
        {
            return Task.Run(() => _repo.GetAllLocationsAsync());
        }

        public Task<List<string>> GetAllDesignationsAsync()
        {
            return Task.Run(() => _repo.GetAllDesignationsAsync());
        }

    }
}
