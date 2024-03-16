using task_api.Models;

namespace task_api.Interfaces;

public interface IWorkRepository
{
    Task<List<Work>> GetAsync();
    Task CreateAsync(Work work);
    Task<Work> GetWorkByIdAsync(string id);
    Task DeleteAsync(string id);
    
    Task<Work> ComprobateUserByTaskIdAsync(string userId, string workId);
    /*IEnumerable<Work> GetAsync();
    void CreateAsync(Work work);
    void DeleteAsync(string id);*/
}