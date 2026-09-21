using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using RTHomePropertyManagement.Controllers;
using RTHomePropertyManagement.DTOs;
using RTHomePropertyManagement.Extensions;
using RTHomePropertyManagement.Models;
using RTHomePropertyManagement.Repositories;

namespace RTHomePropertManagementTests;

public class AgentEndpointsTests
{
    [Fact]
    public async Task ListAgents_ShouldReturnOkResult_WithAgentDtos()
    {
        var agents = new List<Agent>
        {
            new() { Id = 1, Name = "Jane Doe", Designation = "Senior Agent", ImageUrl = "/img.jpg", ProfileLink = null }
        };

        var mockRepo = new Mock<IAgentRepository>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(agents);

        var result = await AgentEndpoints.ListAgents(mockRepo.Object);

        result.Should().BeOfType<Ok<List<AgentDto>>>();
        var okResult = result as Ok<List<AgentDto>>;
        okResult!.Value.Should().BeEquivalentTo(agents.ConvertAll(a => a.ToDto()));
        okResult.Value![0].Link.Should().Be("/agent_details");
    }

    [Fact]
    public async Task GetAgentById_ShouldReturnOkResult_WhenAgentExists()
    {
        var agent = new Agent { Id = 1, Name = "Jane Doe", ProfileLink = "/agent/1", Email = "jane@example.com" };

        var mockRepo = new Mock<IAgentRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(agent);

        var result = await AgentEndpoints.GetAgentById(mockRepo.Object, 1);

        result.Should().BeOfType<Ok<AgentDto>>();
        var okResult = result as Ok<AgentDto>;
        okResult!.Value.Should().BeEquivalentTo(agent.ToDto());
    }

    [Fact]
    public async Task GetAgentById_ShouldReturnNotFound_WhenAgentDoesNotExist()
    {
        var mockRepo = new Mock<IAgentRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Agent?)null);

        var result = await AgentEndpoints.GetAgentById(mockRepo.Object, 1);

        result.Should().BeOfType<NotFound>();
    }
}
