using AutoMapper;
using Domain.Contracts.DTO;
using Domain.Contracts.Request;
using Domain.Contracts.Response;
using Infraestructure.Database.Entities;
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
        Task<List<UserDTO>> ListUsers();
        Task<bool> SaveUser(UserDTO user);
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
        public async Task<List<UserDTO>> ListUsers()
        {
            var users = await _repository.ListUsers();
            return _mapper.Map<List<UserDTO>>(users);
        }
        public async Task<bool> SaveUser(UserDTO user)
        {
            return await _repository.SaveUser(_mapper.Map<User>(user));
        }
    }
}
