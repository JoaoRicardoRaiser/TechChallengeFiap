using AutoMapper;
using TechChallenge.Application.Dtos;
using TechChallenge.Application.Interfaces;
using TechChallenge.Domain.Entities;
using TechChallenge.Domain.Exceptions;
using TechChallenge.Domain.Interfaces.Repositories;

namespace TechChallenge.Application.Services;

public class ContactService(
    IRepository<Contact> contactRepository, 
    IPhoneAreaCache phoneAreaCache,
    IMapper mapper) : IContactService
{
    public async Task<IEnumerable<Contact>> GetAsync(int? phoneAreaCode) // TODO: fazer lógica para trazer do cache de contato ou database.
    {
        return await contactRepository.GetAsync(
            c => phoneAreaCode == null || c.PhoneAreaCode == phoneAreaCode,
            [nameof(Contact.PhoneArea)]);
    }

    public async Task Create(CreateContactDto dto) //TODO: adicionar valor na base de dados e cache de contatos.
    {
        ValidatePhoneAreaCode(dto.Phone);

        var contact = mapper.Map<Contact>(dto);

        await contactRepository.AddAsync(contact);

        await contactRepository.SaveChangesAsync();
    }

    public async Task Update(UpdateContactDto dto) //TODO: sincronizar com o cache de contato.
    {
        ValidatePhoneAreaCode(dto.Phone);

        var contactSaved = await contactRepository.SingleOrDefaultAsync(x => x.Id == dto.ContactId) 
            ?? throw new BusinessException($"Contact not exists. Id: {dto.ContactId}");

        mapper.Map(dto, contactSaved!);

        await contactRepository.SaveChangesAsync();
    }

    private void ValidatePhoneAreaCode(PhoneDto dto)
    {
        var validPhoneAreaCode = phoneAreaCache.ExistsAsync(dto.AreaCode);
        if (!validPhoneAreaCode)
            throw new BusinessException($"Phone area code with code: {dto.AreaCode} not exists!");
    }
}
