using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using RTHomePropertyManagement.Controllers;
using RTHomePropertyManagement.Models;
using RTHomePropertyManagement.DTOs;
using RTHomePropertyManagement.Extensions;
using RTHomePropertyManagement.Repositories;
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
            Title = "Modern Apartment",
            Address = "123 Main St",

            LocationId = 1,
            ListingTypeId = 1,
            AgentId = 1,

            IsForRent = true,
            Price = 1800.00m,
            PricePeriod = "month",

            SquareFeet = 900,
            Bedrooms = 2,
            Bathrooms = 1,
            Status = "ACTIVE"
        };

        var mockRepo = new Mock<IPropertyRepository>();
        mockRepo
            .Setup(r => r.CreateAsync(It.IsAny<Property>()))
            .ReturnsAsync(property);

        // Act
        var result = await PropertyEndpointTestHelpers
            .InvokeCreateProperty(mockRepo.Object, property);

        // Assert
        result.Should().BeOfType<Ok<PropertyDto>>();
        var okResult = result as Ok<PropertyDto>;
        okResult!.Value.Should().BeEquivalentTo(property.ToDto());
    }

    [Fact]
    public async Task ListProperties_ShouldReturnOkResult_WithPropertiesList()
    {
        // Arrange
        var properties = new List<Property>
        {
            new()
            {
                Id = 1,
                Title = "City Condo",
                Address = "10 Broadway",

                LocationId = 1,
                ListingTypeId = 1,
                AgentId = 1,

                IsForRent = true,
                Price = 2200m,
                PricePeriod = "month",
                Bedrooms = 1,
                Bathrooms = 1,
                Status = "ACTIVE"
            },
            new()
            {
                Id = 2,
                Title = "Suburban House",
                Address = "55 Oak Drive",

                LocationId = 2,
                ListingTypeId = 2,
                AgentId = 2,

                IsForRent = false,
                Price = 450000m,
                PricePeriod = "sale",
                Bedrooms = 4,
                Bathrooms = 3,
                Status = "ACTIVE"
            }
        };

        var mockRepo = new Mock<IPropertyRepository>();
        mockRepo
            .Setup(r => r.GetAllAsync(It.IsAny<PropertyFilter>()))
            .ReturnsAsync((properties, properties.Count));

        // Act
        var result = await PropertyEndpointTestHelpers
            .InvokeListProperties(mockRepo.Object);

        // Assert
        result.Should().BeOfType<Ok<List<PropertyDto>>>();
        var okResult = result as Ok<List<PropertyDto>>;
        okResult!.Value.Should().BeEquivalentTo(properties.Select(p => p.ToDto()).ToList());
    }

    [Fact]
    public async Task UpdateProperty_ShouldReturnOkResult_WhenPropertyExists()
    {
        // Arrange
        var existing = new Property
        {
            Id = 1,
            Title = "Old Apartment",
            Address = "123 Main St",
            LocationId = 1,
            ListingTypeId = 1,
            AgentId = 1,
            IsForRent = true,
            Price = 1800.00m,
            PricePeriod = "month",
            SquareFeet = 900,
            Bedrooms = 2,
            Bathrooms = 1,
            Status = "ACTIVE"
        };

        var updated = new Property
        {
            Id = 1,
            Title = "Updated Apartment",
            Address = "123 Main St",

            LocationId = 1,
            ListingTypeId = 1,
            AgentId = 1,

            IsForRent = true,
            Price = 2000m,
            PricePeriod = "month",

            SquareFeet = 950,
            Bedrooms = 2,
            Bathrooms = 2,
            Status = "ACTIVE"
        };

        var mockRepo = new Mock<IPropertyRepository>();
        // Endpoint calls GetByIdAsync first
        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        // Then it calls UpdateAsync with the modified entity — accept any Property
        mockRepo
            .Setup(r => r.UpdateAsync(1, It.IsAny<Property>()))
            .ReturnsAsync(updated);

        // Act
        var result = await PropertyEndpointTestHelpers
            .InvokeUpdateProperty(mockRepo.Object, 1, updated);

        // Assert
        result.Should().BeOfType<Ok<PropertyDto>>();
        var okResult = result as Ok<PropertyDto>;
        okResult!.Value.Should().BeEquivalentTo(updated.ToDto());
    }

    [Fact]
    public async Task UpdateProperty_ShouldReturnNotFound_WhenPropertyDoesNotExist()
    {
        // Arrange
        var updated = new Property
        {
            Title = "Nonexistent Property",
            Address = "Nowhere",

            LocationId = 99,
            ListingTypeId = 99,
            AgentId = 99,

            IsForRent = false,
            Price = 100000m,
            PricePeriod = "sale",
            Status = "ACTIVE"
        };

        var mockRepo = new Mock<IPropertyRepository>();
        // No existing entity
        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Property?)null);
        mockRepo
            .Setup(r => r.UpdateAsync(1, It.IsAny<Property>()))
            .ReturnsAsync((Property?)null);

        // Act
        var result = await PropertyEndpointTestHelpers
            .InvokeUpdateProperty(mockRepo.Object, 1, updated);

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

    [Fact]
    public async Task CreateProperty_ShouldReturnProblem_WhenReferencesAreInvalid()
    {
        // Arrange
        var property = new Property
        {
            Title = "Modern Apartment",
            Address = "123 Main St",
            LocationId = 999,
            IsForRent = true,
            Price = 1800.00m,
            Status = "ACTIVE"
        };

        var mockRepo = new Mock<IPropertyRepository>();
        var mockValidator = new Mock<IReferenceDataValidator>();
        mockValidator
            .Setup(v => v.GetInvalidPropertyReferencesAsync(999, null, null, null))
            .ReturnsAsync(new List<string> { "locationId" });

        // Act
        var result = await PropertyEndpointTestHelpers
            .InvokeCreateProperty(mockRepo.Object, property, mockValidator.Object);

        // Assert - Results.ValidationProblem(...) resolves to ProblemHttpResult
        // at runtime (not the ValidationProblem typed-result), same as
        // Results.Problem(...) used for the 409 conflict case below.
        result.Should().BeOfType<Microsoft.AspNetCore.Http.HttpResults.ProblemHttpResult>();
        var problemResult = result as Microsoft.AspNetCore.Http.HttpResults.ProblemHttpResult;
        problemResult!.StatusCode.Should().Be(400);
        mockRepo.Verify(r => r.CreateAsync(It.IsAny<Property>()), Times.Never);
    }

    [Fact]
    public async Task UpdateProperty_ShouldReturnConflict_WhenConcurrencyConflictOccurs()
    {
        // Arrange
        var existing = new Property
        {
            Id = 1,
            Title = "Old Apartment",
            Address = "123 Main St",
            IsForRent = true,
            Price = 1800.00m,
            Status = "ACTIVE"
        };

        var mockRepo = new Mock<IPropertyRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        mockRepo
            .Setup(r => r.UpdateAsync(1, It.IsAny<Property>()))
            .ThrowsAsync(new ConcurrencyConflictException("Property 1 was modified by another request. Reload and try again."));

        // Act
        var result = await PropertyEndpointTestHelpers
            .InvokeUpdateProperty(mockRepo.Object, 1, existing);

        // Assert
        result.Should().BeOfType<Microsoft.AspNetCore.Http.HttpResults.ProblemHttpResult>();
        var problemResult = result as Microsoft.AspNetCore.Http.HttpResults.ProblemHttpResult;
        problemResult!.StatusCode.Should().Be(409);
    }

    [Fact]
    public async Task ListProperties_ShouldSetTotalCountHeader()
    {
        // Arrange
        var properties = new List<Property>();
        var mockRepo = new Mock<IPropertyRepository>();
        mockRepo
            .Setup(r => r.GetAllAsync(It.IsAny<PropertyFilter>()))
            .ReturnsAsync((properties, 42));

        var response = new Microsoft.AspNetCore.Http.DefaultHttpContext().Response;

        // Act
        var result = await PropertyEndpoints.ListProperties(
            mockRepo.Object,
            response,
            locationId: null,
            listingTypeId: null,
            isForRent: null,
            minPrice: null,
            maxPrice: null,
            minBedrooms: null,
            page: null,
            pageSize: null);

        // Assert
        result.Should().BeOfType<Ok<List<PropertyDto>>>();
        response.Headers["X-Total-Count"].ToString().Should().Be("42");
    }
}
