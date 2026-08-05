using Microsoft.EntityFrameworkCore;
using RTHomePropertyManagement.Models;

namespace RTHomePropertyManagement.Repositories;

public class EstimateRequestRepository : IEstimateRequestRepository
{
    private readonly RealEstateDbContext _dbContext;

    public EstimateRequestRepository(RealEstateDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<EstimateRequest>> GetAllAsync()
    {
        return await _dbContext.EstimateRequests
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<EstimateRequest> CreateAsync(EstimateRequest estimateRequest)
    {
        _dbContext.EstimateRequests.Add(estimateRequest);
        await _dbContext.SaveChangesAsync();
        return estimateRequest;
    }
}
