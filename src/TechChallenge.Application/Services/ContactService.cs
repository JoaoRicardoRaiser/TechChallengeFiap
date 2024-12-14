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
        await ValidateContactAlredySaved(dto);
        ValidatePhoneAreaCode(dto.Phone);

        var contact = mapper.Map<Contact>(dto);

        await contactRepository.AddAsync(contact);

        await contactRepository.SaveChangesAsync();
    }

    public async Task Update(UpdateContactDto dto) //TODO: sincronizar com o cache de contato.
    {
        ValidatePhoneAreaCode(dto.Phone);

        var contactSaved = await GetContactSavedById(dto.ContactId);

        mapper.Map(dto, contactSaved!);

        await contactRepository.SaveChangesAsync();
    }

    public async Task Delete(Guid contactId) //TODO: remover também do cache.
    {
        var contactSaved = await GetContactSavedById(contactId);

        contactRepository.Delete(contactSaved);

        await contactRepository.SaveChangesAsync();
    }

    private async Task ValidateContactAlredySaved(CreateContactDto dto)
    {
        var contactSaved = await contactRepository.SingleOrDefaultAsync(c => c.Name == dto.Name);
        if (contactSaved is not null)
            throw new BusinessException($"Contact with this name alredy exists. Name: {dto.Name}");
    }

    private void ValidatePhoneAreaCode(PhoneDto dto)
    {
        var phoneAreaExists = phoneAreaCache.ExistsAsync(dto.AreaCode);
        if (!phoneAreaExists)
            throw new BusinessException($"Phone area code not exists. Code: {dto.AreaCode}");
    }

    private async Task<Contact> GetContactSavedById(Guid contactId)
        => await contactRepository.SingleOrDefaultAsync(c => c.Id == contactId) ?? throw new BusinessException($"Contact not exists. Id: {contactId}");

}
