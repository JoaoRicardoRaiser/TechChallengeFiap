using Microsoft.AspNetCore.Mvc;
using TechChallenge.Api.Dtos;
using TechChallenge.Application.Dtos;
using TechChallenge.Application.Interfaces;

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
            Phone = new() { Number = dto.PhoneNumber! }
        };

        await contactService.Create(createContactDto);
        return Created();
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromQuery] Guid contactId, [FromBody] PutContactDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updateContactDto = new UpdateContactDto
        {
            ContactId = contactId,
            Email = dto.Email!,
            Phone = new() { Number = dto.PhoneNumber! }
        };

        await contactService.Update(updateContactDto);

        return Accepted();
    }
}
