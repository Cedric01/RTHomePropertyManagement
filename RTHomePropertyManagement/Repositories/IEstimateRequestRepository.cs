using RTHomePropertyManagement.Models;

namespace RTHomePropertyManagement.Repositories;

public interface IEstimateRequestRepository
{
    Task<List<EstimateRequest>> GetAllAsync();
    Task<EstimateRequest> CreateAsync(EstimateRequest estimateRequest);
}
