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
    public interface IPaymentMethodService
    {
        Task<List<PaymentMethodDTO>> ListPaymentMethods();
    }
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IPaymentMethodRepository _repository;
        private readonly IMapper _mapper;

        public PaymentMethodService(IPaymentMethodRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PaymentMethodDTO>> ListPaymentMethods()
        {
            var paymentMethods = await _repository.ListPaymentMethods();
            return _mapper.Map<List<PaymentMethodDTO>>(paymentMethods);
        }
    }
}
