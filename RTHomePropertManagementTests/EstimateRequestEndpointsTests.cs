using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RTHomePropertyManagement.Controllers;
using RTHomePropertyManagement.DTOs;
using RTHomePropertyManagement.Extensions;
using RTHomePropertyManagement.Models;
using RTHomePropertyManagement.Repositories;

namespace RTHomePropertManagementTests;

public class EstimateRequestEndpointsTests
{
    [Fact]
    public async Task ListEstimateRequests_ShouldReturnOkResult_WithRequestsList()
    {
        var requests = new List<EstimateRequest>
        {
            new() { Id = 1, Email = "buyer@example.com" }
        };

        var mockRepo = new Mock<IEstimateRequestRepository>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(requests);

        var result = await EstimateRequestEndpoints.ListEstimateRequests(mockRepo.Object);

        result.Should().BeOfType<Ok<List<EstimateRequest>>>();
        var okResult = result as Ok<List<EstimateRequest>>;
        okResult!.Value.Should().BeEquivalentTo(requests);
    }

    [Fact]
    public async Task CreateEstimateRequest_ShouldTrimEmail_AndReturnOkResult()
    {
        var created = new EstimateRequest { Id = 1, Email = "buyer@example.com" };

        var mockRepo = new Mock<IEstimateRequestRepository>();
        mockRepo
            .Setup(r => r.CreateAsync(It.Is<EstimateRequest>(e => e.Email == "buyer@example.com")))
            .ReturnsAsync(created);

        var result = await EstimateRequestEndpoints.CreateEstimateRequest(
            mockRepo.Object,
            NullLogger<EndpointLogCategory>.Instance,
            new EstimateRequestCreateDto { Email = "  buyer@example.com  " });

        result.Should().BeOfType<Ok<EstimateRequest>>();
        var okResult = result as Ok<EstimateRequest>;
        okResult!.Value.Should().BeEquivalentTo(created);
        mockRepo.Verify(r => r.CreateAsync(It.Is<EstimateRequest>(e => e.Email == "buyer@example.com")), Times.Once);
    }
}
