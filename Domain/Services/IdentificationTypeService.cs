using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts.DTO;
using Infraestructure.Repositories;

namespace Domain.Services
{
    public interface IIdentificationTypeService
    {
        Task<List<IdentificationTypeDTO>> ListIdentificationType();
    }
    public class IdentificationTypeService : IIdentificationTypeService
    {
        private readonly IIdentificationTypeRepository _repository;
        private readonly IMapper _mapper;

        public IdentificationTypeService(IIdentificationTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<IdentificationTypeDTO>> ListIdentificationType()
        {
            var identificationType = await _repository.ListIdentificationType();
            return _mapper.Map<List<IdentificationTypeDTO>>(identificationType);
        }
    }
}
