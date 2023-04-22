using AutoMapper;
using Domain.Contracts.DTO;
using Domain.Contracts.Request;
using Domain.Contracts.Response;
using Infraestructure.Database.Entities;
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
        Task<ProductDTO> GetProductById(int id);
        Task<List<ProductDTO>> ListProducts();
        Task<bool> SaveProduct(ProductDTO product);
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

        public async Task<List<ProductDTO>> ListProducts()
        {
            var products = await _repository.ListProducts();
            return _mapper.Map<List<ProductDTO>>(products);
        }

        public async Task<ProductDTO> GetProductById(int id)
        {
            var product = await _repository.GetProductById(id);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<bool> SaveProduct(ProductDTO product)
        {
            return await _repository.SaveProduct(_mapper.Map<Product>(product));
        }
    }
}
