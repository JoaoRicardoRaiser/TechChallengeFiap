using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TechChallenge.Api.Dtos;
using TechChallenge.Application.Dtos;
using TechChallenge.Application.Interfaces;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Api.Controllers;

[Route("contact")]
public class ContactController(IContactService contactService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int? phoneAreaNumber)
    {
        var contacts = await contactService.GetAsync(phoneAreaNumber);
        return Ok(contacts);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PostContactDto dto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var createContactDto = new CreateContactDto
        {
            Name = dto.Name!,
            Email = dto.Email!,
            PhoneNumber = new() { Number = dto.PhoneNumber! }
        };

        await contactService.Create(createContactDto);
        return Accepted();
    }
}
