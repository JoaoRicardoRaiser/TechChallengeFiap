using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TechChallenge.UpdateContact.Api.Dtos;
using TechChallenge.UpdateContact.Application.Dtos;
using TechChallenge.UpdateContact.Application.Interfaces;

namespace TechChallenge.UpdateContact.Controllers;

[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
[Route("contacts")]
public class ContactController(IContactService contactService, IMapper mapper) : Controller
{
    [HttpPut("{contactId}")]
    public async Task<IActionResult> Put(Guid contactId, [FromBody] PutContactDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updateDto = mapper.Map<UpdateContactDto>(dto);
        updateDto.ContactId = contactId;

        await contactService.UpdateAsync(updateDto);

        return Accepted();
    }
}
