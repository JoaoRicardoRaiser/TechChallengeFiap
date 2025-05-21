using Microsoft.AspNetCore.Mvc;
using TechChallenge.GetContact.Application.Interfaces;


namespace TechChallenge.GetContact.Controllers;

[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
[Route("contacts")]
public class ContactController(IContactService contactService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] int? phoneAreaNumber)
    {
        var contacts = await contactService.GetAsync(phoneAreaNumber);
        return Ok(contacts);
    }
}
