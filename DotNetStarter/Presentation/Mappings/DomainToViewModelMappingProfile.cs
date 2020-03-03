using AutoMapper;
using EPYSLACSCustomer.Core.DTOs;
using EPYSLACSCustomer.Core.Entities;

namespace Presentation.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<TSLLabellingChild, TSLLabellingChildDTO>();
            CreateMap<TSLLabellingMaster, TSLLabellingMasterDTO>()
                .ForMember(dest => dest.Childs, opt => opt.MapFrom(src => src.Childs));

            CreateMap<UKAndCELabellingChild, UkAndCeLabellingChildDTO>();
            CreateMap<UKAndCELabellingMaster, UkAndCeLabellingMasterDTO>()
                .ForMember(dest => dest.Childs, opt => opt.Ignore());
        }
    }
}