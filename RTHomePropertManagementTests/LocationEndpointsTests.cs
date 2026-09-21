using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using RTHomePropertyManagement.Controllers;
using RTHomePropertyManagement.Models;
using RTHomePropertyManagement.Repositories;

namespace RTHomePropertManagementTests;

public class LocationEndpointsTests
{
    [Fact]
    public async Task ListLocations_ShouldReturnOkResult_WithLocationsList()
    {
        var locations = new List<Location>
        {
            new() { Id = 1, DisplayName = "Dhanmondi, Dhaka", City = "Dhanmondi", Country = "Dhaka" }
        };

        var mockRepo = new Mock<ILocationRepository>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(locations);

        var result = await LocationEndpoints.ListLocations(mockRepo.Object);

        result.Should().BeOfType<Ok<List<Location>>>();
        var okResult = result as Ok<List<Location>>;
        okResult!.Value.Should().BeEquivalentTo(locations);
    }

    [Fact]
    public async Task GetLocationById_ShouldReturnOkResult_WhenLocationExists()
    {
        var location = new Location { Id = 1, DisplayName = "Dhanmondi, Dhaka" };

        var mockRepo = new Mock<ILocationRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(location);

        var result = await LocationEndpoints.GetLocationById(mockRepo.Object, 1);

        result.Should().BeOfType<Ok<Location>>();
        var okResult = result as Ok<Location>;
        okResult!.Value.Should().BeEquivalentTo(location);
    }

    [Fact]
    public async Task GetLocationById_ShouldReturnNotFound_WhenLocationDoesNotExist()
    {
        var mockRepo = new Mock<ILocationRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Location?)null);

        var result = await LocationEndpoints.GetLocationById(mockRepo.Object, 1);

        result.Should().BeOfType<NotFound>();
    }
}
