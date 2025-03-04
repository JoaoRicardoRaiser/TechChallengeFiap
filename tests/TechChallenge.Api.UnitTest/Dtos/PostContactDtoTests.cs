using System.ComponentModel.DataAnnotations;
using TechChallenge.Api.Dtos;

namespace TechChallenge.Api.UnitTest.Dtos;

public class PostContactDtoTests
{
    [Fact]
    public void PostContactDto_With_Valid_Data_Should_Passes_Validation()
    {
        // Arrange
        var dto = new PostContactDto
        {
            Name = "John Doe",
            Email = "johndoe@example.com",
            PhoneNumber = "47123456789"
        };

        // Act
        var validationResults = ValidateModel(dto);

        // Assert
        Assert.Empty(validationResults);
    }

    [Theory]
    [InlineData(null, "johndoe@example.com", "47123456789", "The Name field is required.")]
    [InlineData("Jo", "johndoe@example.com", "47123456789", "The field Name must be a string with a minimum length of '3'.")]
    [InlineData("John Doe", null, "471234567890", "The Email field is required.")]
    [InlineData("John Doe", "invalid-email", "471234567890", "The Email field is not a valid e-mail address.")]
    [InlineData("John Doe", "johndoe@example.com", null, "The PhoneNumber field is required.")]
    [InlineData("John Doe", "johndoe@example.com", "777744", "Phone format is invalid. Format accepted is: 99123456789 (DDD+number, without spaces and ())")]
    public void PostContactDto_With_Invalid_Data_Should_Return_ValidationError(
        string name, 
        string email, 
        string phoneNumber, 
        string expectedErrorMessage
    )
    {
        // Arrange
        var dto = new PostContactDto
        {
            Name = name,
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
        var dto = new PostContactDto
        {
            PhoneNumber = "47123456789"
        };

        // Act

        // Assert
        Assert.Equal("47", dto.PhoneAreaCode);
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, context, validationResults, true);
        return validationResults;
    }
}
