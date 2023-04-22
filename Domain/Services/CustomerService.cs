using AutoMapper;
using Domain.Contracts.DTO;
using Infraestructure.Database.Entities;
using Infraestructure.Repositories;


namespace Domain.Services
{
    public interface ICustomerService
    {
        Task<List<CustomerDTO>> ListCustomers();
        Task<CustomerDTO> GetCustomerById(int customerId);
        Task<bool> SaveCustomer(CustomerDTO customer);
    }
    public class CustomerService: ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<CustomerDTO>> ListCustomers()
        {
            var customers = await _repository.ListCustomers();
            return _mapper.Map<List<CustomerDTO>>(customers);
        }
        public async Task<CustomerDTO> GetCustomerById(int customerId)
        {
            var customer = await _repository.GetCustomerById(customerId);
            return _mapper.Map<CustomerDTO>(customer);
        }
        public async Task<bool> SaveCustomer(CustomerDTO customer)
        {
            return await _repository.SaveCustomer(_mapper.Map<Customer>(customer));
        }
    }
}
