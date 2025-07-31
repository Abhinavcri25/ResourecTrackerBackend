//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using ResourceTracker.Models;
//using ResourceTracker.Orchestration;
//using Microsoft.Extensions.Logging;

//namespace ResourecTracker.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ResourceController : ControllerBase
//    {
//        private readonly IResourceService _service;
//        private readonly ILogger<ResourceController> _logger;
//        public ResourceController(IResourceService service, ILogger<ResourceController> logger)
//        {
//            _service = service;
//            _logger = logger;
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Resource>>> GetAll()
//        {
//            _logger.LogInformation("Fetching all Resources");
//            try
//            {
//                var res = (await _service.GetAll());
//                return Ok(res);
//            }
//            catch(Exception ex)
//            {
//                _logger.LogError(ex, "An error occured while fetching Resources");
//                return StatusCode(500, "Internal Server error");
//            }
//        }
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Resource>> Get(int id)
//        {
//            var result = await _service.Get(id);
//            if (result == null) return NotFound();
//            return Ok(result);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Add([FromBody] Resource resource)
//        {
//            var result = await _service.Add(resource);
//            return result ? Ok() : BadRequest();
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, [FromBody] Resource resource)
//        {
//            //if (id != resource.EmpId) return BadRequest("ID mismatch");
//            var result = await _service.Update(resource);
//            return result ? Ok() : NotFound();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var result = await _service.Delete(id);
//            return result ? Ok() : NotFound();
//        }
//    }
//}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceTracker.Models;
using ResourceTracker.Orchestration;
using Microsoft.Extensions.Logging;

namespace ResourecTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly IResourceOrchestration _service;
        private readonly ILogger<ResourceController> _logger;

        public ResourceController(IResourceOrchestration service, ILogger<ResourceController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet(APIUri.GetAllEmployee)]
        public async Task<ActionResult<IEnumerable<Resource>>> GetAll()
        {
            _logger.LogInformation("Fetching all Resources");
            try
            {
                var res = await _service.GetAll();
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all resources.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet(APIUri.GetEmployeeByID)]
        public async Task<ActionResult<Resource>> Get([FromQuery] int id)
        {
            _logger.LogInformation($"Fetching resource with ID: {id}");
            try
            {
                var result = await _service.Get(id);
                _logger.LogInformation($"{id}");
                if (result == null)
                {
                    _logger.LogWarning($"Resource with ID {id} not found.");
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching resource with ID {id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost(APIUri.InsertEmployee)]
        public async Task<IActionResult> Add([FromBody] Resource resource)
        {
            _logger.LogInformation("Adding a new resource.");
            try
            {
                var result = await _service.Add(resource);
                if (result)
                {
                    _logger.LogInformation("Resource added successfully.");
                    return Ok();
                }
                else
                {
                    _logger.LogWarning("Failed to add resource.");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a resource.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut(APIUri.UpdateEmployee + "/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Resource resource)
        {
            _logger.LogInformation($"Updating resource with ID: {id}");
            try
            {
                // if (id != resource.EmpId) return BadRequest("ID mismatch");
                var result = await _service.Update(resource);
                //if (result)
                //{
                //    _logger.LogInformation($"Resource with ID {id} updated successfully.");
                //    return Ok();
                //}
                //else
                //{
                //    _logger.LogWarning($"Resource with ID {id} not found for update.");
                //    return NotFound();
                //} 
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating resource with ID {id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete(APIUri.DeleteEmployee)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            _logger.LogInformation($"Deleting resource with ID: {id}");
            try
            {
                var result = await _service.Delete(id);
                if (result)
                {
                    _logger.LogInformation($"Resource with ID {id} deleted successfully.");
                    return Ok();
                }
                else
                {
                    _logger.LogWarning($"Resource with ID {id} not found for deletion.");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting resource with ID {id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }
        //[HttpGet("checkemail")]
        //public async Task<IActionResult> CheckEmail([FromQuery] string email)
        //{
        //    _logger.LogInformation("Checking if email exists: {Email}", email);

        //    try
        //    {
        //        var resource = await _service.GetByEmail(email);
        //        if (resource != null)
        //        {
        //            _logger.LogWarning("Email {Email} already exists.", email);
        //            return Conflict("Email already exists."); // 409
        //        }

        //        return Ok(); // 200 - Email is available
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while checking email: {Email}", email);
        //        return StatusCode(500, "Internal Server Error");
        //    }
        //}

        [HttpGet(APIUri.Skills)]
        public async Task<ActionResult<List<string>>> GetAllSkillsAsync()
        {
            _logger.LogInformation("Fetching all skills from service.");
            try
            {
                var skills = await _service.GetAllSkillsAsync();
                _logger.LogInformation("Successfully fetched {Count} skills.", skills.Count);
                return Ok(skills);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching skills.");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet(APIUri.Locations)]
        public async Task<ActionResult<List<string>>> GetAllLocationsAsync()
        {
            _logger.LogInformation("Fetching all locations from service.");
            try
            {
                var locations = await _service.GetAllLocationsAsync();
                _logger.LogInformation("Successfully fetched {Count} locations.", locations.Count);
                return Ok(locations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching locations.");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet(APIUri.Designations)]
        public async Task<ActionResult<List<string>>> GetAllDesignationsAsync()
        {
            _logger.LogInformation("Fetching all designations from service.");
            try
            {
                var designations = await _service.GetAllDesignationsAsync();
                _logger.LogInformation("Successfully fetched {Count} designations.", designations.Count);
                return Ok(designations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching designations.");
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
