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
    public interface IPaymentService
    {
        Task<List<PaymentDTO>> ListPaymentBySale(int saleId);
    }
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repository;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PaymentDTO>> ListPaymentBySale(int saleId)
        {
            var payments = await _repository.ListPaymentBySale(saleId);
            return _mapper.Map<List<PaymentDTO>>(payments);
        }
    }
}
