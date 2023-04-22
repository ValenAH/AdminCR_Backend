using AutoMapper;
using Domain.Contracts.DTO;
using Infraestructure.Database.Entities;
using Infraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface ISaleService
    {
        Task<List<SaleDTO>> ListSales();
        Task<SaleDTO> GetSaleById(int saleId);
        Task<bool> SaveSale(SaleDTO sale);
    }
    public class SaleService: ISaleService
    {
        private readonly ISaleRepository _repository;
        private readonly IMapper _mapper;

        public SaleService(ISaleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<SaleDTO>> ListSales()
        {
            var sales = await _repository.ListSales();
            return _mapper.Map<List<SaleDTO>>(sales);
        }
        public async Task<SaleDTO> GetSaleById(int saleId)
        {
            var sale = await _repository.GetCustomerById(saleId);
            return _mapper.Map<SaleDTO>(sale);
        }
        public async Task<bool> SaveSale(SaleDTO sale)
        {
            return await _repository.SaveSale(_mapper.Map<Sale>(sale));
        }
    }
}
