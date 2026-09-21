using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RTHomePropertyManagement.Controllers;
using RTHomePropertyManagement.Extensions;
using RTHomePropertyManagement.Models;
using RTHomePropertyManagement.Repositories;

namespace RTHomePropertManagementTests;

public class PriceRangeEndpointsTests
{
    [Fact]
    public async Task ListPriceRanges_ShouldReturnOkResult_WithPriceRangesList()
    {
        var ranges = new List<PriceRange>
        {
            new() { Id = 1, MinPrice = 1000, MaxPrice = 2000, DisplayLabel = "$1,000 - $2,000" }
        };

        var mockRepo = new Mock<IPriceRangeRepository>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(ranges);

        var result = await PriceRangeEndpoints.ListPriceRanges(mockRepo.Object);

        result.Should().BeOfType<Ok<List<PriceRange>>>();
        var okResult = result as Ok<List<PriceRange>>;
        okResult!.Value.Should().BeEquivalentTo(ranges);
    }

    [Fact]
    public async Task CreatePriceRange_ShouldReturnOkResult_WithCreatedPriceRange()
    {
        var priceRange = new PriceRange { Id = 1, MinPrice = 1000, MaxPrice = 2000, DisplayLabel = "$1,000 - $2,000" };

        var mockRepo = new Mock<IPriceRangeRepository>();
        mockRepo.Setup(r => r.CreateAsync(It.IsAny<PriceRange>())).ReturnsAsync(priceRange);

        var result = await PriceRangeEndpoints.CreatePriceRange(
            mockRepo.Object,
            NullLogger<EndpointLogCategory>.Instance,
            priceRange);

        result.Should().BeOfType<Ok<PriceRange>>();
        var okResult = result as Ok<PriceRange>;
        okResult!.Value.Should().BeEquivalentTo(priceRange);
    }
}
