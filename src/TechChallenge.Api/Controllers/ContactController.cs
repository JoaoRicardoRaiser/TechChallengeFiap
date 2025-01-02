using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TechChallenge.Api.Dtos;
using TechChallenge.Application.Dtos;
using TechChallenge.Application.Interfaces;

namespace TechChallenge.Api.Controllers;

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

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PostContactDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createContactDto = mapper.Map<CreateContactDto>(dto);

        await contactService.CreateAsync(createContactDto);

        return Accepted();
    }

    [HttpPut("{contactId}")]
    public async Task<IActionResult> Put(Guid contactId, [FromBody] PutContactDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updateContactDto = mapper.Map<UpdateContactDto>(dto);
        updateContactDto.ContactId = contactId;

        await contactService.UpdateAsync(updateContactDto);

        return Accepted();
    }

    [HttpDelete("{contactId}")]
    public async Task<IActionResult> Delete(Guid contactId)
    {
        await contactService.DeleteAsync(contactId);

        return NoContent();
    }
}
