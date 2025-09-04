using AntiFraudAPI.DTO.Request;
using AntiFraudAPI.DTO.Response;
using AutoMapper;
using Domain;

namespace AntiFraudAPI.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<OperationRequest, Operation>()
                .ForMember(dest => dest.IdCustomer, opt => opt.MapFrom(src => src.TargetAccountId))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.Id, y => y.Ignore())
                .ForMember(dest => dest.OperationDate, y => y.Ignore())
                .ForMember(dest => dest.TotalAmount, y => y.Ignore());

            CreateMap<Operation, OperationResponse>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.OperationDate))
                .ForMember(dest => dest.TransactionExternalId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
