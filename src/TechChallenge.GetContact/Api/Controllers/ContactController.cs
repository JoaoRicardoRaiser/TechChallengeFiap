using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TechChallenge.UpdateContact.Application.Interfaces;


namespace TechChallenge.UpdateContact.Controllers;

[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
[Route("contacts")]
public class ContactController(IContactService contactService, IMapper mapper) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int? phoneAreaNumber)
    {
        var contacts = await contactService.GetAsync(phoneAreaNumber);
        return Ok(contacts);
    }

}
