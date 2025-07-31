

//using ResourceTracker.Models;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Threading.Tasks;
//using Microsoft.Data.SqlClient;
//using Microsoft.Extensions.Logging;

//namespace ResourceTracker.DAO
//{
//    public class ResourceDAO : IResourceDAO
//    {
//        private readonly SqlConnect _DbContext;
//        private readonly ILogger<ResourceDAO> _logger;

//        public ResourceDAO(SqlConnect dbContext, ILogger<ResourceDAO> logger)
//        {
//            _DbContext = dbContext;
//            _logger = logger;
//        }

//        private void AddParameters(SqlCommand cmd, Resource resource)
//        {
//            cmd.Parameters.AddWithValue("EmpId", resource.EmpId);
//            cmd.Parameters.AddWithValue("ResourceName", resource.ResourceName);
//            cmd.Parameters.AddWithValue("Designation", resource.Designation);
//            cmd.Parameters.AddWithValue("ReportingTo", resource.ReportingTo);
//            cmd.Parameters.AddWithValue("Billable", resource.Billable);
//            cmd.Parameters.AddWithValue("TechnologySkill", resource.TechnologySkill);
//            cmd.Parameters.AddWithValue("ProjectAllocation", resource.ProjectAllocation);
//            cmd.Parameters.AddWithValue("Location", resource.Location);
//            cmd.Parameters.AddWithValue("EmailId", resource.EmailId);
//            cmd.Parameters.AddWithValue("CteDoj", resource.CteDoj);
//            cmd.Parameters.AddWithValue("Remarks", resource.Remarks);
//        }

//        public async Task<List<Resource>> GetAll()
//        {
//            _logger.LogInformation("Fetching all resources from database.");
//            var resources = new List<Resource>();

//            try
//            {
//                using var conn = _DbContext.GetConnection();
//                using var cmd = new SqlCommand("sp_GetAllResources", conn)
//                {
//                    CommandType = CommandType.StoredProcedure
//                };

//                await conn.OpenAsync();
//                using var reader = await cmd.ExecuteReaderAsync();

//                while (await reader.ReadAsync())
//                {
//                    resources.Add(new Resource
//                    {
//                        EmpId = (int)reader["EmpId"],
//                        ResourceName = reader["ResourceName"].ToString(),
//                        Designation = reader["Designation"].ToString(),
//                        ReportingTo = reader["ReportingTo"].ToString(),
//                        Billable = (bool)reader["Billable"],
//                        TechnologySkill = reader["TechnologySkill"].ToString(),
//                        ProjectAllocation = reader["ProjectAllocation"].ToString(),
//                        Location = reader["Location"].ToString(),
//                        EmailId = reader["EmailId"].ToString(),
//                        CteDoj = DateOnly.FromDateTime(Convert.ToDateTime(reader["CteDoj"])),
//                        Remarks = reader["Remarks"].ToString()
//                    });
//                }

//                _logger.LogInformation("Successfully fetched {Count} resources.", resources.Count);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while fetching all resources.");
//                throw;
//            }

//            return resources;
//        }

//        public async Task<Resource> Get(int id)
//        {
//            _logger.LogInformation("Fetching resource with ID: {Id}", id);
//            Resource temp = null;

//            try
//            {
//                using var conn = _DbContext.GetConnection();
//                using var cmd = new SqlCommand("sp_GetResourceById", conn)
//                {
//                    CommandType = CommandType.StoredProcedure
//                };

//                cmd.Parameters.AddWithValue("EmpId", id);
//                await conn.OpenAsync();

//                using var reader = await cmd.ExecuteReaderAsync();
//                if (await reader.ReadAsync())
//                {
//                    temp = new Resource
//                    {
//                        EmpId = (int)reader["EmpId"],
//                        ResourceName = reader["ResourceName"].ToString(),
//                        Designation = reader["Designation"].ToString(),
//                        ReportingTo = reader["ReportingTo"].ToString(),
//                        Billable = (bool)reader["Billable"],
//                        TechnologySkill = reader["TechnologySkill"].ToString(),
//                        ProjectAllocation = reader["ProjectAllocation"].ToString(),
//                        Location = reader["Location"].ToString(),
//                        EmailId = reader["EmailId"].ToString(),
//                        CteDoj = DateOnly.FromDateTime(Convert.ToDateTime(reader["CteDoj"])),
//                        Remarks = reader["Remarks"].ToString()
//                    };

//                    _logger.LogInformation("Resource with ID {Id} fetched successfully.", id);
//                }
//                else
//                {
//                    _logger.LogWarning("Resource with ID {Id} not found.", id);
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while fetching resource with ID: {Id}", id);
//                throw;
//            }

//            return temp;
//        }

//        public async Task<bool> update(Resource resource)
//        {
//            _logger.LogInformation("Updating resource with ID: {Id}", resource.EmpId);

//            try
//            {
//                using var conn = _DbContext.GetConnection();
//                using var cmd = new SqlCommand("sp_UpdateResource", conn)
//                {
//                    CommandType = CommandType.StoredProcedure
//                };

//                AddParameters(cmd, resource);
//                await conn.OpenAsync();
//                int rows = await cmd.ExecuteNonQueryAsync();

//                if (rows > 0)
//                {
//                    _logger.LogInformation("Resource with ID {Id} updated successfully.", resource.EmpId);
//                    return true;
//                }
//                else
//                {
//                    _logger.LogWarning("No resource found to update with ID: {Id}", resource.EmpId);
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while updating resource with ID: {Id}", resource.EmpId);
//                throw;
//            }
//        }

//        public async Task<bool> delete(int id)
//        {
//            _logger.LogInformation("Deleting resource with ID: {Id}", id);

//            try
//            {
//                using var conn = _DbContext.GetConnection();
//                using var cmd = new SqlCommand("sp_DeleteResource", conn)
//                {
//                    CommandType = CommandType.StoredProcedure
//                };

//                cmd.Parameters.AddWithValue("EmpId", id);
//                await conn.OpenAsync();

//                int rows = await cmd.ExecuteNonQueryAsync();

//                if (rows > 0)
//                {
//                    _logger.LogInformation("Resource with ID {Id} deleted successfully.", id);
//                    return true;
//                }
//                else
//                {
//                    _logger.LogWarning("No resource found to delete with ID: {Id}", id);
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while deleting resource with ID: {Id}", id);
//                throw;
//            }
//        }

//        public async Task<bool> Add(Resource resource)
//        {
//            _logger.LogInformation("Inserting a new resource.");

//            try
//            {
//                using var conn = _DbContext.GetConnection();
//                using var cmd = new SqlCommand("sp_InsertResource", conn)
//                {
//                    CommandType = CommandType.StoredProcedure
//                };

//                cmd.Parameters.AddWithValue("ResourceName", resource.ResourceName);
//                cmd.Parameters.AddWithValue("Designation", resource.Designation);
//                cmd.Parameters.AddWithValue("ReportingTo", resource.ReportingTo);
//                cmd.Parameters.AddWithValue("Billable", resource.Billable);
//                cmd.Parameters.AddWithValue("TechnologySkill", resource.TechnologySkill);
//                cmd.Parameters.AddWithValue("ProjectAllocation", resource.ProjectAllocation);
//                cmd.Parameters.AddWithValue("Location", resource.Location);
//                cmd.Parameters.AddWithValue("EmailId", resource.EmailId);
//                cmd.Parameters.AddWithValue("CteDoj", resource.CteDoj);
//                cmd.Parameters.AddWithValue("Remarks", resource.Remarks);

//                await conn.OpenAsync();
//                object result = await cmd.ExecuteScalarAsync();

//                if (result != null && int.TryParse(result.ToString(), out int newEmpId))
//                {
//                    resource.EmpId = newEmpId;
//                    _logger.LogInformation("Resource inserted with ID: {Id}", newEmpId);
//                    return true;
//                }

//                _logger.LogWarning("Resource insert failed.");
//                return false;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error occurred while inserting a new resource.");
//                throw;
//            }
//        }

//        public async Task<Resource> GetByEmail(string email)
//        {
//            _logger.LogInformation("Checking for resource with email: {Email}", email);

//            Resource temp = null;

//            try
//            {
//                using var conn = _DbContext.GetConnection();
//                using var cmd = new SqlCommand("sp_GetResourceByEmail", conn)
//                {
//                    CommandType = CommandType.StoredProcedure
//                };

//                cmd.Parameters.AddWithValue("EmailId", email);
//                await conn.OpenAsync();

//                using var reader = await cmd.ExecuteReaderAsync();
//                if (await reader.ReadAsync())
//                {
//                    temp = new Resource
//                    {
//                        EmpId = (int)reader["EmpId"],
//                        ResourceName = reader["ResourceName"].ToString(),
//                        Designation = reader["Designation"].ToString(),
//                        ReportingTo = reader["ReportingTo"].ToString(),
//                        Billable = (bool)reader["Billable"],
//                        TechnologySkill = reader["TechnologySkill"].ToString(),
//                        ProjectAllocation = reader["ProjectAllocation"].ToString(),
//                        Location = reader["Location"].ToString(),
//                        EmailId = reader["EmailId"].ToString(),
//                        CteDoj = DateOnly.FromDateTime(Convert.ToDateTime(reader["CteDoj"])),
//                        Remarks = reader["Remarks"].ToString()
//                    };
//                }

//                return temp;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error while checking email: {Email}", email);
//                throw;
//            }
//        }

//    }
//}



using ResourceTracker.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace ResourceTracker.DAO
{
    public class ResourceDAO : IResourceDAO
    {
        private readonly SqlConnect _DbContext;
        private readonly ILogger<ResourceDAO> _logger;

        public ResourceDAO(SqlConnect dbContext, ILogger<ResourceDAO> logger)
        {
            _DbContext = dbContext;
            _logger = logger;
        }

        private void AddParameters(SqlCommand cmd, Resource resource)
        {
            cmd.Parameters.AddWithValue("EmpId", resource.EmpId);
            cmd.Parameters.AddWithValue("ResourceName", resource.ResourceName);
            cmd.Parameters.AddWithValue("Designation", resource.Designation);
            cmd.Parameters.AddWithValue("ReportingTo", resource.ReportingTo);
            cmd.Parameters.AddWithValue("Billable", resource.Billable);
            cmd.Parameters.AddWithValue("TechnologySkill", resource.TechnologySkill);
            cmd.Parameters.AddWithValue("ProjectAllocation", resource.ProjectAllocation);
            cmd.Parameters.AddWithValue("Location", resource.Location);
            cmd.Parameters.AddWithValue("EmailId", resource.EmailId);
            cmd.Parameters.AddWithValue("CteDoj", resource.CteDoj);
            cmd.Parameters.AddWithValue("Remarks", resource.Remarks);
        }

        public async Task<List<Resource>> GetAll()
        {
            _logger.LogInformation("Fetching all resources from database.");
            var resources = new List<Resource>();

            try
            {
                using var conn = _DbContext.GetConnection();
                using var cmd = new SqlCommand("sp_GetAllResources", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    resources.Add(new Resource
                    {
                        EmpId = (int)reader["EmpId"],
                        ResourceName = reader["ResourceName"].ToString(),
                        Designation = reader["Designation"].ToString(),
                        ReportingTo = reader["ReportingTo"].ToString(),
                        Billable = (bool)reader["Billable"],
                        TechnologySkill = reader["TechnologySkill"].ToString(),
                        ProjectAllocation = reader["ProjectAllocation"].ToString(),
                        Location = reader["Location"].ToString(),
                        EmailId = reader["EmailId"].ToString(),
                        CteDoj = DateOnly.FromDateTime(Convert.ToDateTime(reader["CteDoj"])),
                        Remarks = reader["Remarks"].ToString()
                    });
                }

                _logger.LogInformation("Successfully fetched {Count} resources.", resources.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all resources.");
                throw;
            }

            return resources;
        }

        public async Task<Resource> Get(int id)
        {
            _logger.LogInformation("Fetching resource with ID: {Id}", id);
            Resource temp = null;

            try
            {
                using var conn = _DbContext.GetConnection();
                using var cmd = new SqlCommand("sp_GetResourceById", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("EmpId", id);
                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    temp = new Resource
                    {
                        EmpId = (int)reader["EmpId"],
                        ResourceName = reader["ResourceName"].ToString(),
                        Designation = reader["Designation"].ToString(),
                        ReportingTo = reader["ReportingTo"].ToString(),
                        Billable = (bool)reader["Billable"],
                        TechnologySkill = reader["TechnologySkill"].ToString(),
                        ProjectAllocation = reader["ProjectAllocation"].ToString(),
                        Location = reader["Location"].ToString(),
                        EmailId = reader["EmailId"].ToString(),
                        CteDoj = DateOnly.FromDateTime(Convert.ToDateTime(reader["CteDoj"])),
                        Remarks = reader["Remarks"].ToString()
                    };

                    _logger.LogInformation("Resource with ID {Id} fetched successfully.", id);
                }
                else
                {
                    _logger.LogWarning("Resource with ID {Id} not found.", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching resource with ID: {Id}", id);
                throw;
            }

            return temp;
        }

        //public async Task<bool> update(Resource resource)
        //{
        //    _logger.LogInformation("Updating resource with ID: {Id}", resource.EmpId);

        //    try
        //    {
        //        using var conn = _DbContext.GetConnection();
        //        await conn.OpenAsync();

        //        using (var cmd = new SqlCommand("sp_UpdateResource", conn))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            AddParameters(cmd, resource);
        //            await cmd.ExecuteNonQueryAsync();
        //        }

        //        using (var deleteCmd = new SqlCommand("sp_DeleteResourceSkills", conn))
        //        {
        //            deleteCmd.CommandType = CommandType.StoredProcedure;
        //            deleteCmd.Parameters.AddWithValue("EmpId", resource.EmpId);
        //            await deleteCmd.ExecuteNonQueryAsync();
        //        }

        //        foreach (var skill in resource.TechnologySkill.Split(',', StringSplitOptions.RemoveEmptyEntries))
        //        {
        //            int skillId = 0;
        //            using (var skillCmd = new SqlCommand("sp_EnsureSkillExists", conn))
        //            {
        //                skillCmd.CommandType = CommandType.StoredProcedure;
        //                skillCmd.Parameters.AddWithValue("SkillName", skill.Trim());

        //                var output = new SqlParameter("SkillId", SqlDbType.Int)
        //                {
        //                    Direction = ParameterDirection.Output
        //                };
        //                skillCmd.Parameters.Add(output);

        //                await skillCmd.ExecuteNonQueryAsync();
        //                skillId = (int)output.Value;
        //            }

        //            using (var mapCmd = new SqlCommand("sp_InsertResourceSkill", conn))
        //            {
        //                mapCmd.CommandType = CommandType.StoredProcedure;
        //                mapCmd.Parameters.AddWithValue("EmpId", resource.EmpId);
        //                mapCmd.Parameters.AddWithValue("SkillId", skillId);
        //                await mapCmd.ExecuteNonQueryAsync();
        //            }
        //        }

        //        _logger.LogInformation("Resource with ID {Id} updated successfully.", resource.EmpId);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while updating resource with ID: {Id}", resource.EmpId);
        //        throw;
        //    }
        //}
        public async Task<bool> update(Resource resource)
        {
            _logger.LogInformation("Updating resource with ID: {Id}", resource.EmpId);

            try
            {
                using var conn = _DbContext.GetConnection();
                await conn.OpenAsync();

                // 1. Update main Resource table
                using (var cmd = new SqlCommand("sp_UpdateResource", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Hardcoded parameters matching exactly with stored procedure
                    cmd.Parameters.AddWithValue("@EmpId", resource.EmpId);
                    cmd.Parameters.AddWithValue("@ResourceName", resource.ResourceName);
                    cmd.Parameters.AddWithValue("@Designation", resource.Designation);
                    cmd.Parameters.AddWithValue("@ReportingTo", resource.ReportingTo);
                    cmd.Parameters.AddWithValue("@Billable", resource.Billable);
                    cmd.Parameters.AddWithValue("@Location", resource.Location);
                    cmd.Parameters.AddWithValue("@EmailId", resource.EmailId);
                    cmd.Parameters.AddWithValue("@CteDoj", resource.CteDoj);
                    cmd.Parameters.AddWithValue("@ProjectAllocation", resource.ProjectAllocation);
                    cmd.Parameters.AddWithValue("@Remarks", resource.Remarks ?? string.Empty); // in case it's null

                    await cmd.ExecuteNonQueryAsync();
                }

                // 2. Delete old skills
                using (var deleteCmd = new SqlCommand("sp_DeleteResourceSkills", conn))
                {
                    deleteCmd.CommandType = CommandType.StoredProcedure;
                    deleteCmd.Parameters.AddWithValue("@EmpId", resource.EmpId);
                    await deleteCmd.ExecuteNonQueryAsync();
                }

                // 3. Insert updated skills
                foreach (var skill in resource.TechnologySkill.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    int skillId = 0;

                    using (var skillCmd = new SqlCommand("sp_EnsureSkillExists", conn))
                    {
                        skillCmd.CommandType = CommandType.StoredProcedure;
                        skillCmd.Parameters.AddWithValue("@SkillName", skill.Trim());

                        var output = new SqlParameter("@SkillId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        skillCmd.Parameters.Add(output);

                        await skillCmd.ExecuteNonQueryAsync();
                        skillId = (int)output.Value;
                    }

                    using (var mapCmd = new SqlCommand("sp_InsertResourceSkill", conn))
                    {
                        mapCmd.CommandType = CommandType.StoredProcedure;
                        mapCmd.Parameters.AddWithValue("@EmpId", resource.EmpId);
                        mapCmd.Parameters.AddWithValue("@SkillId", skillId);
                        await mapCmd.ExecuteNonQueryAsync();
                    }
                }

                _logger.LogInformation("Resource with ID {Id} updated successfully.", resource.EmpId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating resource with ID: {Id}", resource.EmpId);
                throw;
            }
        }


        public async Task<bool> delete(int id)
        {
            _logger.LogInformation("Deleting resource with ID: {Id}", id);

            try
            {
                using var conn = _DbContext.GetConnection();
                using var cmd = new SqlCommand("sp_DeleteResource", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("EmpId", id);
                await conn.OpenAsync();

                int rows = await cmd.ExecuteNonQueryAsync();

                if (rows > 0)
                {
                    _logger.LogInformation("Resource with ID {Id} deleted successfully.", id);
                    return true;
                }
                else
                {
                    _logger.LogWarning("No resource found to delete with ID: {Id}", id);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting resource with ID: {Id}", id);
                throw;
            }
        }

        public async Task<bool> Add(Resource resource)
        {
            _logger.LogInformation("Inserting a new resource.");

            try
            {
                using var conn = _DbContext.GetConnection();
                await conn.OpenAsync();

                int newEmpId = 0;
                using (var cmd = new SqlCommand("sp_InsertResource", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("ResourceName", resource.ResourceName);
                    cmd.Parameters.AddWithValue("Designation", resource.Designation);
                    cmd.Parameters.AddWithValue("ReportingTo", resource.ReportingTo);
                    cmd.Parameters.AddWithValue("Billable", resource.Billable);
                    cmd.Parameters.AddWithValue("Location", resource.Location);
                    cmd.Parameters.AddWithValue("EmailId", resource.EmailId);
                    cmd.Parameters.AddWithValue("CteDoj", resource.CteDoj);
                    cmd.Parameters.AddWithValue("ProjectAllocation", resource.ProjectAllocation);
                    cmd.Parameters.AddWithValue("Remarks", resource.Remarks);

                    object result = await cmd.ExecuteScalarAsync();

                    if (result != null && int.TryParse(result.ToString(), out newEmpId))
                    {
                        resource.EmpId = newEmpId;
                        _logger.LogInformation("Resource inserted with ID: {Id}", newEmpId);
                    }
                    else
                    {
                        _logger.LogWarning("Resource insert failed.");
                        return false;
                    }
                }

                foreach (var skill in resource.TechnologySkill.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    int skillId = 0;
                    using (var skillCmd = new SqlCommand("sp_EnsureSkillExists", conn))
                    {
                        skillCmd.CommandType = CommandType.StoredProcedure;
                        skillCmd.Parameters.AddWithValue("SkillName", skill.Trim());

                        var output = new SqlParameter("SkillId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        skillCmd.Parameters.Add(output);

                        await skillCmd.ExecuteNonQueryAsync();
                        skillId = (int)output.Value;
                    }

                    using (var mapCmd = new SqlCommand("sp_InsertResourceSkill", conn))
                    {
                        mapCmd.CommandType = CommandType.StoredProcedure;
                        mapCmd.Parameters.AddWithValue("EmpId", newEmpId);
                        mapCmd.Parameters.AddWithValue("SkillId", skillId);
                        await mapCmd.ExecuteNonQueryAsync();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while inserting a new resource.");
                throw;
            }
        }

        public async Task<Resource> GetByEmail(string email)
        {
            _logger.LogInformation("Checking for resource with email: {Email}", email);

            Resource temp = null;

            try
            {
                using var conn = _DbContext.GetConnection();
                using var cmd = new SqlCommand("sp_GetResourceByEmail", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("EmailId", email);
                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    temp = new Resource
                    {
                        EmpId = (int)reader["EmpId"],
                        ResourceName = reader["ResourceName"].ToString(),
                        Designation = reader["Designation"].ToString(),
                        ReportingTo = reader["ReportingTo"].ToString(),
                        Billable = (bool)reader["Billable"],
                        TechnologySkill = reader["TechnologySkill"].ToString(),
                        ProjectAllocation = reader["ProjectAllocation"].ToString(),
                        Location = reader["Location"].ToString(),
                        EmailId = reader["EmailId"].ToString(),
                        CteDoj = DateOnly.FromDateTime(Convert.ToDateTime(reader["CteDoj"])),
                        Remarks = reader["Remarks"].ToString()
                    };
                }

                return temp;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while checking email: {Email}", email);
                throw;
            }
        }

        public async Task<List<string>> GetAllDesignationsAsync()
        {
            _logger.LogInformation("Fetching all designations.");
            var list = new List<string>();

            try
            {
                using var conn = _DbContext.GetConnection();
                using var cmd = new SqlCommand("sp_GetAllDesignations", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(reader["Name"].ToString());
                }

                _logger.LogInformation("Fetched {Count} designations.", list.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching designations.");
                throw;
            }

            return list;
        }

        public async Task<List<string>> GetAllLocationsAsync()
        {
            _logger.LogInformation("Fetching all locations.");
            var list = new List<string>();

            try
            {
                using var conn = _DbContext.GetConnection();
                using var cmd = new SqlCommand("sp_GetAllLocations", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(reader["Name"].ToString());
                }

                _logger.LogInformation("Fetched {Count} locations.", list.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching locations.");
                throw;
            }

            return list;
        }

        public async Task<List<string>> GetAllSkillsAsync()
        {
            _logger.LogInformation("Fetching all skills.");
            var list = new List<string>();

            try
            {
                using var conn = _DbContext.GetConnection();
                using var cmd = new SqlCommand("sp_GetAllSkills", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                await conn.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(reader["SkillName"].ToString());
                }

                _logger.LogInformation("Fetched {Count} skills.", list.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching skills.");
                throw;
            }

            return list;
        }
    }
}


