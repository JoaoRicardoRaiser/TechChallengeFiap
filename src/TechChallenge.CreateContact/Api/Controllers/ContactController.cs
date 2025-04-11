using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TechChallenge.CreateContact.Api.Dtos;
using TechChallenge.CreateContact.Application.Dtos;
using TechChallenge.CreateContact.Application.Interfaces;

namespace TechChallenge.CreateContact.Controllers;

[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
[Route("contacts")]
public class ContactController(IContactService contactService, IMapper mapper) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PostContactDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createContactDto = mapper.Map<CreateContactDto>(dto);

        await contactService.CreateAsync(createContactDto);

        return Accepted();
    }
}
