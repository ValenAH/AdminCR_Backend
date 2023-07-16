using AutoMapper;
using Domain.Contracts.DTO;
using Infraestructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Config
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<ProductDTO, Product>().ReverseMap();
            CreateMap<CategoryDTO, Category>().ReverseMap();
            CreateMap<CustomerDTO, Customer>().ReverseMap();
            CreateMap<IdentificationTypeDTO, IdentificationType>().ReverseMap();
            CreateMap<SaleDTO, Sale>().ReverseMap();
            CreateMap<SaleStatusDTO,SaleStatus>().ReverseMap();
            CreateMap<SaleDetailsDTO, SaleDetails>().ReverseMap();
            CreateMap<PaymentMethodDTO, PaymentMethod>().ReverseMap();
            CreateMap<PaymentDTO, Payment>().ReverseMap();
        }
    }
}
