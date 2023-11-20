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
        Task<List<SaleDTO>> ListCreditSales();
        Task<SaleDTO> GetSaleById(int saleId);
        Task<bool> UpdateSale(SaleDTO sale);
        Task<int> SaveSale(SaleDTO sale);
        Task<List<SaleDTO>> GetSalesByCustomerId(int customerId);
        Task<string> GetConsecutive();
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
        public async Task<List<SaleDTO>> ListCreditSales()
        {
            var creditSales = await _repository.ListCreditSales();
            return _mapper.Map<List<SaleDTO>>(creditSales);
        }
        public async Task<SaleDTO> GetSaleById(int saleId)
        {
            var sale = await _repository.GetSaleById(saleId);
            return _mapper.Map<SaleDTO>(sale);
        }
        public async Task<bool> UpdateSale(SaleDTO sale)
        {
            return await _repository.UpdateSale(_mapper.Map<Sale>(sale));
        }
        public async Task<int> SaveSale(SaleDTO sale)
        {
            return await _repository.SaveSale(_mapper.Map<Sale>(sale));
        }
        public async Task<List<SaleDTO>> GetSalesByCustomerId(int customerId)
        {
            var sales = await _repository.GetSalesByCustomerId(customerId);
            return _mapper.Map<List<SaleDTO>>(sales);
        }

        public async Task<string> GetConsecutive()
        {
            var salesCount = await _repository.GetConsecutive();
            var invoiceNumber = "CR00000";
            salesCount++;
            string salesNumber = salesCount.ToString();
            invoiceNumber = invoiceNumber.Remove(7 - salesNumber.Length, salesNumber.Length) + salesNumber;
            return invoiceNumber;
        }
    }
}
