using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourceTracker.Models;


namespace ResourceTracker.DAO
{
    public interface IResourceDAO
    {
        public Task<List<Resource>> GetAll();   
        public Task<Resource> Get(int id);
        public Task<bool> update(Resource resource);
        public Task<bool> delete(int id); 
        public Task<bool> Add(Resource resource);
        public Task<Resource> GetByEmail(string email);

        public Task<List<string>> GetAllDesignationsAsync();
        public Task<List<string>> GetAllLocationsAsync();
        public Task<List<string>> GetAllSkillsAsync();

    }
}
