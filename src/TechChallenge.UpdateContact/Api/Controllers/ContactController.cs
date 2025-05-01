using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TechChallenge.UpdateContact.Application.Interfaces;
using TechChallenge.UpdateContact.Api.Dtos;
using TechChallenge.UpdateContact.Application.Dtos;


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

        var updateContactDto = mapper.Map<UpdateContactDto>(dto);
        updateContactDto.ContactId = contactId;

        await contactService.UpdateAsync(updateContactDto);

        return Accepted();
    }

}
