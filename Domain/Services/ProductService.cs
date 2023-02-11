using AutoMapper;
using Domain.Contracts.DTO;
using Domain.Contracts.Request;
using Domain.Contracts.Response;
using Infraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IProductService
    {
        Task<ProductDTO> GetProductById(ProductRequest req);
    }
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ProductDTO> GeProductById(ProductRequest req)
        {
            return _mapper.Map<ProductDTO>(await _repository.GetProductById(req.IdProduct));
        }
    }
}
