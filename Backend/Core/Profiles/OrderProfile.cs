using AutoMapper;
using Core.Entities;
using Core.Models;

namespace Core.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderModel, Order>().ReverseMap();
    }
}