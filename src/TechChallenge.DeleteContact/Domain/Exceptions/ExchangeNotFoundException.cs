namespace TechChallenge.DeleteContact.Domain.Exceptions;

public class ExchangeNotFoundException(string publisherKey) : Exception($"No exchange configured for publisher key: {publisherKey}")
{
}
