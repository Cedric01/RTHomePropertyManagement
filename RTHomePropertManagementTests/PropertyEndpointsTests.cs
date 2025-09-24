using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using RTHomePropertyManagement.Controllers;
using RTHomePropertyManagement.Models;
using RTHomePropertManagementTests;

namespace RTHomePropertManagementTests;

public class PropertyEndpointsTests
{
    [Fact]
    public async Task CreateProperty_ShouldReturnOkResult_WithCreatedProperty()
    {
        // Arrange

        var property = new Property { Title = "Test", Description = "Desc", Price = 100, Bedrooms = 2, Bathrooms = 1 };
        var mockSet = new Mock<DbSet<Property>>();
        var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        mockContext.Setup(m => m.Add(It.IsAny<Property>())).Callback<Property>(p => { });
        mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await PropertyEndpointTestHelpers.InvokeCreateProperty(mockContext.Object, property);

        // Assert
        result.Should().BeOfType<Ok<object>>();
        var okResult = result as Ok<object>;
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
        }.AsQueryable();

        var mockSet = new Mock<DbSet<Property>>();
        mockSet.As<IQueryable<Property>>().Setup(m => m.Provider).Returns(properties.Provider);
        mockSet.As<IQueryable<Property>>().Setup(m => m.Expression).Returns(properties.Expression);
        mockSet.As<IQueryable<Property>>().Setup(m => m.ElementType).Returns(properties.ElementType);
        mockSet.As<IQueryable<Property>>().Setup(m => m.GetEnumerator()).Returns(properties.GetEnumerator());
        mockSet.Setup(m => m.ToListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(properties.ToList());

        var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        mockContext.Setup(m => m.Properties).Returns(mockSet.Object);

        // Act
        var result = await PropertyEndpointTestHelpers.InvokeListProperties(mockContext.Object);

        // Assert
        result.Should().BeOfType<Ok<object>>();
        var okResult = result as Ok<object>;
        okResult?.Value.Should().BeEquivalentTo(properties.ToList());
    }

    [Fact]
    public async Task UpdateProperty_ShouldReturnOkResult_WhenPropertyExists()
    {
        // Arrange
        var property = new Property { Id = 1, Title = "Old", Description = "Old", Price = 1, Bedrooms = 1, Bathrooms = 1 };
        var updated = new Property { Id = 1, Title = "New", Description = "New", Price = 2, Bedrooms = 2, Bathrooms = 2 };

        var mockSet = new Mock<DbSet<Property>>();
        mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(property);

        var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        mockContext.Setup(m => m.Properties).Returns(mockSet.Object);
        mockContext.Setup(m => m.Entry(It.IsAny<Property>())).Returns(Mock.Of<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Property>>());
        mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await PropertyEndpointTestHelpers.InvokeUpdateProperty(mockContext.Object, 1, updated);

        // Assert
        result.Should().BeOfType<Ok<object>>();
        var okResult = result as Ok<object>;
        okResult?.Value.Should().BeEquivalentTo(updated, options => options.Excluding(p => p.Id));
    }

    [Fact]
    public async Task UpdateProperty_ShouldReturnNotFound_WhenPropertyDoesNotExist()
    {
        // Arrange
        var updated = new Property { Id = 1, Title = "New", Description = "New", Price = 2, Bedrooms = 2, Bathrooms = 2 };
        var mockSet = new Mock<DbSet<Property>>();
        mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync((Property)null);

        var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        mockContext.Setup(m => m.Properties).Returns(mockSet.Object);

        // Act
        var result = await PropertyEndpointTestHelpers.InvokeUpdateProperty(mockContext.Object, 1, updated);

        // Assert
        result.Should().BeOfType<NotFound>();
    }

    [Fact]
    public async Task DeleteProperty_ShouldReturnNoContent_WhenPropertyExists()
    {
        // Arrange
        var property = new Property { Id = 1, Title = "A", Description = "A", Price = 1, Bedrooms = 1, Bathrooms = 1 };
        var mockSet = new Mock<DbSet<Property>>();
        mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync(property);
        mockSet.Setup(m => m.Remove(It.IsAny<Property>()));

        var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        mockContext.Setup(m => m.Properties).Returns(mockSet.Object);
        mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        var result = await PropertyEndpointTestHelpers.InvokeDeleteProperty(mockContext.Object, 1);

        // Assert
        result.Should().BeOfType<NoContent>();
    }

    [Fact]
    public async Task DeleteProperty_ShouldReturnNotFound_WhenPropertyDoesNotExist()
    {
        // Arrange
        var mockSet = new Mock<DbSet<Property>>();
        mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).ReturnsAsync((Property)null);

        var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        mockContext.Setup(m => m.Properties).Returns(mockSet.Object);

        // Act
        var result = await PropertyEndpointTestHelpers.InvokeDeleteProperty(mockContext.Object, 1);

        // Assert
        result.Should().BeOfType<NotFound>();
    }
}
