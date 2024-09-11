namespace DemoBlazor.Services;

using AutoMapper;
using DeltaX.Core.Common;
using DemoBlazor.Database.Entities;
using DemoBlazor.Shared.Entities;

public class MapperService
{
    IMapper mapper;

    public MapperService()
    {
        var config = new MapperConfiguration(conf =>
        {
            conf.CreateMap(typeof(Pagination<>), typeof(Pagination<>));
            conf.CreateMap<Tour, TourDto>().ReverseMap();
        });

        mapper = config.CreateMapper();
    }

    public TourDto ToDto(Tour e) => mapper.Map<TourDto>(e);

    public Pagination<TourDto> ToDto(Pagination<Tour> e) => mapper.Map<Pagination<TourDto>>(e);
}
