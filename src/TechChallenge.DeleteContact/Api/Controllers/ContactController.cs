using Microsoft.AspNetCore.Mvc;
using TechChallenge.DeleteContact.Application.Interfaces;

namespace TechChallenge.DeleteContact.Api.Controllers;

[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
[Route("contacts")]
public class ContactController(IContactService contactService) : Controller
{
    [HttpDelete("{contactId}")]
    public async Task<IActionResult> DeleteAsync(Guid contactId)
    {
        await contactService.DeleteAsync(contactId);

        return NoContent();
    }
}