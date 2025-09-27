using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using RTHomePropertyManagement.Controllers;
using RTHomePropertyManagement.Models;

namespace RTHomePropertManagementTests;

public class PropertyEndpointsTests
{
    [Fact]
    public async Task CreateProperty_ShouldReturnOkResult_WithCreatedProperty()
    {
        // Arrange
        var property = new Property { Title = "Test", Description = "Desc", Price = 100, Bedrooms = 2, Bathrooms = 1 };
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
            new Property { Id = 1, Title = "A", Description = "A", Price = 1, Bedrooms = 1, Bathrooms = 1 },
            new Property { Id = 2, Title = "B", Description = "B", Price = 2, Bedrooms = 2, Bathrooms = 2 }
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
        var property = new Property { Id = 1, Title = "Old", Description = "Old", Price = 1, Bedrooms = 1, Bathrooms = 1 };
        var updated = new Property { Id = 1, Title = "New", Description = "New", Price = 2, Bedrooms = 2, Bathrooms = 2 };
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
        var updated = new Property { Id = 1, Title = "New", Description = "New", Price = 2, Bedrooms = 2, Bathrooms = 2 };
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
