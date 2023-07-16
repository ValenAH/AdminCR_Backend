using AutoMapper;
using Domain.Contracts.DTO;
using Infraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> ListCategories();
    }
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<CategoryDTO>> ListCategories()
        {
            var categories = await _repository.ListCategories();
            return _mapper.Map<List<CategoryDTO>>(categories);
        }
    }
}
