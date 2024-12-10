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

    public async Task<IEnumerable<Contact>> GetAsync(int? phoneAreaCode)
        => await contactRepository.GetAsync(
                c => phoneAreaCode == null || c.PhoneAreaCode == phoneAreaCode,
                [nameof(Contact.PhoneArea)]
            );

    public async Task Create(CreateContactDto dto)
    {
        var validPhoneAreaCode = phoneAreaCache.ExistsAsync(dto.PhoneNumber.PhoneAreaCode);
        if (!validPhoneAreaCode)
            throw new BusinessException($"Phone area code with code: {dto.PhoneNumber.PhoneAreaCode} not exists!");

        var contact = mapper.Map<Contact>(dto);

        await contactRepository.AddAsync(contact);

        await contactRepository.SaveChangesAsync();
    }

}
