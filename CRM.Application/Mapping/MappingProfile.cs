using AutoMapper;
using CRM.Application.DTOs;
using CRM.Domain.Entities;

namespace CRM.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Customer, CustomerDto>();
        }
    }
} 