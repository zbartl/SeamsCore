namespace SeamsCore.Features.Page
{
    using AutoMapper;
    using SeamsCore.Domain;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PageSlot, Load.Slot>();
            CreateMap<Page, Load.Result>();
        }
    }
}
