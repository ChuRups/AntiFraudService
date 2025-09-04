using AutoMapper;
using Domain;
using Infrastructure.DTO;

namespace AntiFraudTransaction.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<OperationRequest, TransactionalOperation>()
                .ForMember(dest => dest.Id, y => y.Ignore())
                .ForMember(dest => dest.IdState, y => y.Ignore());
        }
    }
}
