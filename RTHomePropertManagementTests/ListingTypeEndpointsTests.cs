using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using RTHomePropertyManagement.Controllers;
using RTHomePropertyManagement.Models;
using RTHomePropertyManagement.Repositories;

namespace RTHomePropertManagementTests;

public class ListingTypeEndpointsTests
{
    [Fact]
    public async Task ListListingTypes_ShouldReturnOkResult_WithListingTypesList()
    {
        var listingTypes = new List<ListingType>
        {
            new() { Id = 1, Code = "rent", Label = "For Rent" }
        };

        var mockRepo = new Mock<IListingTypeRepository>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(listingTypes);

        var result = await ListingTypeEndpoints.ListListingTypes(mockRepo.Object);

        result.Should().BeOfType<Ok<List<ListingType>>>();
        var okResult = result as Ok<List<ListingType>>;
        okResult!.Value.Should().BeEquivalentTo(listingTypes);
    }

    [Fact]
    public async Task GetListingTypeById_ShouldReturnOkResult_WhenListingTypeExists()
    {
        var listingType = new ListingType { Id = 1, Code = "rent", Label = "For Rent" };

        var mockRepo = new Mock<IListingTypeRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(listingType);

        var result = await ListingTypeEndpoints.GetListingTypeById(mockRepo.Object, 1);

        result.Should().BeOfType<Ok<ListingType>>();
        var okResult = result as Ok<ListingType>;
        okResult!.Value.Should().BeEquivalentTo(listingType);
    }

    [Fact]
    public async Task GetListingTypeById_ShouldReturnNotFound_WhenListingTypeDoesNotExist()
    {
        var mockRepo = new Mock<IListingTypeRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((ListingType?)null);

        var result = await ListingTypeEndpoints.GetListingTypeById(mockRepo.Object, 1);

        result.Should().BeOfType<NotFound>();
    }
}
