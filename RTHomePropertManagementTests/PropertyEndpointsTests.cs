using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using RTHomePropertyManagement.Controllers;
using RTHomePropertyManagement.Models;
using RTHomePropertyManagementTests;

namespace RTHomePropertManagementTests;

public class PropertyEndpointsTests
{
    [Fact]
    public async Task CreateProperty_ShouldReturnOkResult_WithCreatedProperty()
    {
        // Arrange
        var property = new Property
        {
            Name = "Test",
            City = "Los Angeles",
            State = "CA",
            Photo = "photo_url",
            AvailableUnits = 5,
            Wifi = true,
            Laundry = true
        };
        var mockRepo = new Mock<IPropertyRepository>();
        mockRepo.Setup(r => r.CreateAsync(It.IsAny<Property>())).ReturnsAsync(property);

        // Act
        var result = await PropertyEndpointTestHelpers.InvokeCreateProperty(mockRepo.Object, property);

        // Assert
        result.Should().BeOfType<Ok<Property>>();
        var okResult = result as Ok<Property>;
        okResult?.Value.Should().BeEquivalentTo(property);
    }

    [Fact]
    public async Task ListProperties_ShouldReturnOkResult_WithPropertiesList()
    {
        // Arrange
        var properties = new List<Property>
        {
            new Property
            {
                Id = 1,
                Name = "Location A",
                City = "New York",
                State = "NY",
                Photo = "photo_a_url",
                AvailableUnits = 2,
                Wifi = false,
                Laundry = true
            },
            new Property
            {
                Id = 2,
                Name = "Location B",
                City = "Chicago",
                State = "IL",
                Photo = "photo_b_url",
                AvailableUnits = 3,
                Wifi = true,
                Laundry = false
            }
        };
        var mockRepo = new Mock<IPropertyRepository>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(properties);

        // Act
        var result = await PropertyEndpointTestHelpers.InvokeListProperties(mockRepo.Object);

        // Assert
        result.Should().BeOfType<Ok<List<Property>>>();
        var okResult = result as Ok<List<Property>>;
        okResult?.Value.Should().BeEquivalentTo(properties);
    }

    [Fact]
    public async Task UpdateProperty_ShouldReturnOkResult_WhenPropertyExists()
    {
        // Arrange
        var property = new Property
        {
            Id = 1,
            Name = "Old Location",
            City = "Miami",
            State = "FL",
            Photo = "old_photo_url",
            AvailableUnits = 1,
            Wifi = false,
            Laundry = false
        };
        var updated = new Property
        {
            Id = 1,
            Name = "Updated Location",
            City = "Miami",
            State = "FL",
            Photo = "updated_photo_url",
            AvailableUnits = 4,
            Wifi = true,
            Laundry = true
        };
        var mockRepo = new Mock<IPropertyRepository>();
        mockRepo.Setup(r => r.UpdateAsync(1, updated)).ReturnsAsync(updated);

        // Act
        var result = await PropertyEndpointTestHelpers.InvokeUpdateProperty(mockRepo.Object, 1, updated);

        // Assert
        result.Should().BeOfType<Ok<Property>>();
        var okResult = result as Ok<Property>;
        okResult?.Value.Should().BeEquivalentTo(updated, options => options.Excluding(p => p.Id));
    }

    [Fact]
    public async Task UpdateProperty_ShouldReturnNotFound_WhenPropertyDoesNotExist()
    {
        // Arrange
        var updated = new Property 
        { 
            Id = 1,
            
                Name = "Test",
                City = "Desc",
                State = "CA",
                Photo = "td",
                AvailableUnits = 1,
                Laundry = true,
                Wifi = true
            };
            var mockRepo = new Mock<IPropertyRepository>();
        mockRepo.Setup(r => r.UpdateAsync(1, updated)).ReturnsAsync((Property?)null);

        // Act
        var result = await PropertyEndpointTestHelpers.InvokeUpdateProperty(mockRepo.Object, 1, updated);

        // Assert
        result.Should().BeOfType<NotFound>();
    }

    [Fact]
    public async Task DeleteProperty_ShouldReturnNoContent_WhenPropertyExists()
    {
        // Arrange
        var mockRepo = new Mock<IPropertyRepository>();
        mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await PropertyEndpointTestHelpers.InvokeDeleteProperty(mockRepo.Object, 1);

        // Assert
        result.Should().BeOfType<NoContent>();
    }

    [Fact]
    public async Task DeleteProperty_ShouldReturnNotFound_WhenPropertyDoesNotExist()
    {
        // Arrange
        var mockRepo = new Mock<IPropertyRepository>();
        mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(false);

        // Act
        var result = await PropertyEndpointTestHelpers.InvokeDeleteProperty(mockRepo.Object, 1);

        // Assert
        result.Should().BeOfType<NotFound>();
    }
}
