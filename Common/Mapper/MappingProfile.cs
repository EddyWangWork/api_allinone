using AutoMapper;
using demoAPI.Model.DS;

namespace demoAPI.Common.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DSTransaction, DSTransactionDto>();
            CreateMap<DSTransactionReq, DSTransaction>();
        }
    }
}
