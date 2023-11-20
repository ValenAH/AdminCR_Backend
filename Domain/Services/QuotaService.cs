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
    public interface IQuotaService
    {
        Task<List<QuotaDTO>> ListQuotas();
        Task<QuotaDTO> GetQuotaBySaleId(int id);
        Task<bool> UpdateQuota(QuotaDTO quotaDTO);
        Task<bool> SaveQuota(QuotaDTO quotaDTO);
    }
    public class QuotaService : IQuotaService
    {
        private readonly IQuotaRepository _repository;
        private readonly IMapper _mapper;

        public QuotaService(IQuotaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<QuotaDTO>> ListQuotas()
        {
            var quotas = await _repository.ListQuotas();
            return _mapper.Map<List<QuotaDTO>>(quotas);
        }

        public async Task<QuotaDTO> GetQuotaBySaleId(int id)
        {
            var quota = await _repository.GetQuotaBySaleId(id);
            return _mapper.Map<QuotaDTO>(quota);
        }

        public async Task<bool> UpdateQuota(QuotaDTO quota)
        {
            return await _repository.UpdateQuota(_mapper.Map<Quota>(quota));
        }

        public async Task<bool> SaveQuota(QuotaDTO quota)
        {
            return await _repository.SaveQuota(_mapper.Map<Quota>(quota));
        }
    }
}
