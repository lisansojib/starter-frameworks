using AutoMapper;
using EPYSLACSCustomer.Core.Entities;
using Presentation.Models;

namespace Presentation.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<TSLLabellingChildBindingModel, TSLLabellingChild>();
            CreateMap<TSLLabellingMasterBindingModel, TSLLabellingMaster>()
                .ForMember(dest => dest.Childs, opt=> opt.MapFrom(src => src.Childs));

            CreateMap<UkAndCeLabellingChildBindingModel, UKAndCELabellingChild>();
            CreateMap<UkAndCeLabellingMasterBindingModel, UKAndCELabellingMaster>()
                .ForMember(dest => dest.Childs, opt => opt.MapFrom(src => src.Childs));
        }
    }
}