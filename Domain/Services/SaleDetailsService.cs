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
    public interface ISaleDetailsService
    {
        Task<List<SaleDetailsDTO>> ListSaleDetails(int saleId);
    }
    public class SaleDetailsService : ISaleDetailsService
    {
        private readonly ISaleDetailsRepository _repository;
        private readonly IMapper _mapper;

        public SaleDetailsService (ISaleDetailsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<SaleDetailsDTO>> ListSaleDetails(int saleId)
        {
            var saleDetails = await _repository.ListSaleDetails(saleId);
            return _mapper.Map<List<SaleDetailsDTO>>(saleDetails);
        }
    }
}
