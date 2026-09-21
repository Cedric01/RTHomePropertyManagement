using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentAssertions;
using RTHomePropertyManagement.DTOs;
using RTHomePropertyManagement.Models;

namespace RTHomePropertManagementTests;

// Exercises the DataAnnotations placed on the request DTOs/entities directly
// (the same Validator.TryValidateObject call ValidationFilter<T> makes at
// request time), independent of the ASP.NET Core filter pipeline.
public class ValidationTests
{
    private static List<ValidationResult> Validate(object instance)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(instance, new ValidationContext(instance), results, validateAllProperties: true);
        return results;
    }

    [Fact]
    public void PropertyCreateDto_ShouldFailValidation_WhenTitleIsMissing()
    {
        var dto = new PropertyCreateDto
        {
            Title = string.Empty,
            Address = "123 Main St",
            IsForRent = true,
            Price = 1800.00m
        };

        var results = Validate(dto);

        results.Should().Contain(r => r.MemberNames.Contains("Title"));
    }

    [Fact]
    public void PropertyCreateDto_ShouldFailValidation_WhenPriceIsZeroOrNegative()
    {
        var dto = new PropertyCreateDto
        {
            Title = "Modern Apartment",
            Address = "123 Main St",
            IsForRent = true,
            Price = 0m
        };

        var results = Validate(dto);

        results.Should().Contain(r => r.MemberNames.Contains("Price"));
    }

    [Fact]
    public void PropertyCreateDto_ShouldPassValidation_WhenAllRequiredFieldsAreValid()
    {
        var dto = new PropertyCreateDto
        {
            Title = "Modern Apartment",
            Address = "123 Main St",
            IsForRent = true,
            Price = 1800.00m,
            PricePeriod = "month"
        };

        var results = Validate(dto);

        results.Should().BeEmpty();
    }

    [Fact]
    public void EstimateRequestCreateDto_ShouldFailValidation_WhenEmailIsInvalid()
    {
        var dto = new EstimateRequestCreateDto { Email = "not-an-email" };

        var results = Validate(dto);

        results.Should().Contain(r => r.MemberNames.Contains("Email"));
    }

    [Fact]
    public void EstimateRequestCreateDto_ShouldPassValidation_WhenEmailIsValid()
    {
        var dto = new EstimateRequestCreateDto { Email = "buyer@example.com" };

        var results = Validate(dto);

        results.Should().BeEmpty();
    }

    [Fact]
    public void PriceRange_ShouldFailValidation_WhenMaxPriceIsBelowMinPrice()
    {
        var priceRange = new PriceRange { MinPrice = 2000, MaxPrice = 1000, DisplayLabel = "Bad Range" };

        var results = Validate(priceRange);

        results.Should().Contain(r => r.MemberNames.Contains("MaxPrice"));
    }

    [Fact]
    public void PriceRange_ShouldPassValidation_WhenMaxPriceIsAtLeastMinPrice()
    {
        var priceRange = new PriceRange { MinPrice = 1000, MaxPrice = 2000, DisplayLabel = "$1,000 - $2,000" };

        var results = Validate(priceRange);

        results.Should().BeEmpty();
    }
}
