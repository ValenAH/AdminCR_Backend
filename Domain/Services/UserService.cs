using AutoMapper;
using Domain.Contracts.DTO;
using Domain.Contracts.Request;
using Domain.Contracts.Response;
using Infraestructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByCredentials(LoginRequest req);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<UserDTO> GetUserByCredentials(LoginRequest req)
        {
            return _mapper.Map<UserDTO>(await _repository.GetUserByCredentials(req.UserName, req.Password));
        }
    }
}
