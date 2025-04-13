namespace Pulse.Infrastructure.Profiles
{
    using AutoMapper;
    using Pulse.Core.Models.Entities;
    using Pulse.Core.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Venue, VenueItem>()
                .ForMember(dest => dest.VenueTypeName, opt => opt.MapFrom(src => src.VenueType != null ? src.VenueType.Name : null));

            CreateMap<Venue, VenueWithDetails>()
                .IncludeBase<Venue, VenueItem>()
                .ForMember(dest => dest.BusinessHours, opt => opt.MapFrom(src => src.BusinessHours))
                .ForMember(dest => dest.Specials, opt => opt.MapFrom(src => src.Specials));

            CreateMap<NewVenueRequest, Venue>();
            CreateMap<UpdateVenueRequest, Venue>();

            CreateMap<VenueType, VenueTypeItem>();
            CreateMap<NewVenueTypeRequest, VenueType>();
            CreateMap<UpdateVenueTypeRequest, VenueType>();

            CreateMap<Special, SpecialItem>();

            CreateMap<Special, SpecialWithDetails>()
                .IncludeBase<Special, SpecialItem>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Tag)))
                .ForMember(dest => dest.Venue, opt => opt.MapFrom(src => src.Venue));

            CreateMap<Special, SpecialWithVenue>()
                .IncludeBase<Special, SpecialItem>()
                .ForMember(dest => dest.Venue, opt => opt.MapFrom(src => src.Venue));

            CreateMap<NewSpecialRequest, Special>();
            CreateMap<UpdateSpecialRequest, Special>();

            CreateMap<Tag, TagItem>();
            CreateMap<NewTagRequest, Tag>()
                .ForMember(dest => dest.UsageCount, opt => opt.MapFrom(_ => 0));
            CreateMap<UpdateTagRequest, Tag>()
                .ForMember(dest => dest.UsageCount, opt => opt.Ignore());

            CreateMap<OperatingSchedule, OperatingScheduleItem>();
            CreateMap<NewOperatingScheduleRequest, OperatingSchedule>();
            CreateMap<UpdateOperatingScheduleRequest, OperatingSchedule>();

            CreateMap<(Venue Venue, double DistanceMiles, IEnumerable<Special> ActiveSpecials), VenueWithActiveSpecials>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Venue.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Venue.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Venue.Description))
                .ForMember(dest => dest.AddressLine1, opt => opt.MapFrom(src => src.Venue.AddressLine1))
                .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(src => src.Venue.AddressLine2))
                .ForMember(dest => dest.AddressLine3, opt => opt.MapFrom(src => src.Venue.AddressLine3))
                .ForMember(dest => dest.AddressLine4, opt => opt.MapFrom(src => src.Venue.AddressLine4))
                .ForMember(dest => dest.Locality, opt => opt.MapFrom(src => src.Venue.Locality))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Venue.Region))
                .ForMember(dest => dest.Postcode, opt => opt.MapFrom(src => src.Venue.Postcode))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Venue.Country))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Venue.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Venue.Email))
                .ForMember(dest => dest.Website, opt => opt.MapFrom(src => src.Venue.Website))
                .ForMember(dest => dest.ImageLink, opt => opt.MapFrom(src => src.Venue.ImageLink))
                .ForMember(dest => dest.VenueTypeId, opt => opt.MapFrom(src => src.Venue.VenueTypeId))
                .ForMember(dest => dest.VenueTypeName, opt => opt.MapFrom(src => src.Venue.VenueType != null ? src.Venue.VenueType.Name : null))
                .ForMember(dest => dest.DistanceMiles, opt => opt.MapFrom(src => src.DistanceMiles))
                .ForMember(dest => dest.ActiveSpecials, opt => opt.MapFrom(src => src.ActiveSpecials));
        }
    }
}
