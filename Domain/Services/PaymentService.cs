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
    public interface IPaymentService
    {
        Task<List<PaymentDTO>> ListPaymentBySale(int saleId);
        Task<bool> SavePayment(List<PaymentDTO> payment);
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

        public async Task<bool> SavePayment(List<PaymentDTO> payment)
        {
            return await _repository.SavePayment(_mapper.Map<List<Payment>>(payment));
        }
    }
}
