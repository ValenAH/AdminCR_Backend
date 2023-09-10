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
    public interface ISaleDetailsService
    {
        Task<List<SaleDetailsDTO>> ListSaleDetails(int saleId);
        Task<bool> SaveSaleDetails(List<SaleDetailsDTO> list);
        Task<bool> UpdateSaleDetail(SaleDetailsDTO saleDetail);
        Task<bool> DeleteSaleDetail(int saleDetailId);
    }
    public class SaleDetailsService : ISaleDetailsService
    {
        private readonly ISaleDetailsRepository _repository;
        private readonly ISaleService _saleService;
        private readonly IMapper _mapper;

        public SaleDetailsService (ISaleDetailsRepository repository, ISaleService saleService, IMapper mapper)
        {
            _repository = repository;
            _saleService = saleService;
            _mapper = mapper;
        }
        public async Task<List<SaleDetailsDTO>> ListSaleDetails(int saleId)
        {
            var saleDetails = await _repository.ListSaleDetails(saleId);
            return _mapper.Map<List<SaleDetailsDTO>>(saleDetails);
        }

        public async Task<bool> UpdateSaleDetail(SaleDetailsDTO saleDetail)
        {
            return await _repository.UpdateSaleDetail(_mapper.Map<SaleDetails>(saleDetail)); ;
        }

        public async Task<bool> SaveSaleDetails(List<SaleDetailsDTO> list)
        {
            var saleDetails = await _repository.SaveSaleDetails(_mapper.Map<List<SaleDetails>>(list));
            return saleDetails;
        }

        public async Task<bool> DeleteSaleDetail(int saleDetailId)
        {
            var saleId = await _repository.DeleteSaleDetail(saleDetailId);
            var sale = await _saleService.GetSaleById(saleId);
            bool changed = false;
            if (sale != null)
            {
                decimal totalAmount = 0;

                foreach (var saleDetail in sale.SaleDetails)
                {
                    totalAmount += saleDetail.Amount * saleDetail.Quantity;
                }
                sale.TotalAmount = totalAmount;
                changed = await _saleService.UpdateSale(sale);

            }
            return changed;
        }
    }
}
