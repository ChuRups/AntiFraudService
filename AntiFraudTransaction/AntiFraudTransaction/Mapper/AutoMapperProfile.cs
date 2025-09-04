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

            CreateMap<TransactionalOperation, OperationResponse>()
                .ForMember(dest => dest.TransactionExternalId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
