using AutoMapper;
using Core.Entities;
using Core.Models;

namespace Core.Profiles;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<CustomerModel, Customer>()
              .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
              {
                  AddressLine = src.AddressLine,
                  AddressLine2 = src.AddressLine2,
                  City = src.City,
                  State = src.State,
                  Country = src.Country
              }))
              .ReverseMap();
    }
}