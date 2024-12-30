using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechChallenge.Api.Dtos;

namespace TechChallenge.Api.UnitTest.Dtos;
public class PutContactDtoTests
{

    [Fact]
    public void PutContactDto_With_Valid_Data_Should_Passes_Validation()
    {
        // Arrange
        var dto = new PutContactDto
        {
            Email = "johndoe@example.com",
            PhoneNumber = "47123456789"
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        Assert.Empty(validationResults);
    }

    [Theory]
    [InlineData(null, "471234567890", "The Email field is required.")]
    [InlineData("invalid-email", "471234567890", "The Email field is not a valid e-mail address.")]
    [InlineData("johndoe@example.com", null, "The PhoneNumber field is required.")]
    [InlineData("johndoe@example.com", "7117788", "Phone format is invalid. Format accepted is: 99123456789 (DDD+number, without spaces and ())")]
    public void PutContactDto_With_Invalid_Data_Should_Return_ValidationError(
        string email,
        string phoneNumber,
        string expectedErrorMessage
    )
    {
        // Arrange
        var dto = new PutContactDto
        {
            Email = email,
            PhoneNumber = phoneNumber
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        Assert.Contains(validationResults, v => v.ErrorMessage == expectedErrorMessage);
    }

    [Fact]
    public void PhoneAreaCode_Should_Returns_Correct_Value()
    {
        // Arrange
        var dto = new PutContactDto
        {
            PhoneNumber = "47123456789"
        };

        // Act
        var areaCode = dto.PhoneAreaCode;

        // Assert
        Assert.Equal("47", areaCode);
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, context, validationResults, true);
        return validationResults;
    }
}

