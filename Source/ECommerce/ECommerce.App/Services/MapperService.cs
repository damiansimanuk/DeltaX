namespace ECommerce.App.Services;

using AutoMapper;
using DeltaX.Core.Common;
using ECommerce.App.Database.Entities;
using ECommerce.Shared.Entities;

public class MapperService
{
    IMapper mapper;

    public MapperService()
    {
        var config = new MapperConfiguration(conf =>
        {
            conf.CreateMap(typeof(Pagination<>), typeof(Pagination<>));
            conf.CreateMap<Category, CategoryDto>().ReverseMap();
            conf.CreateMap<ProductDetail, ProductDetailDto>().ReverseMap();
            conf.CreateMap<Product, ProductDto>().ReverseMap();
            conf.CreateMap<Seller, SellerDto>().ReverseMap();
            conf.CreateMap<Stock, StockDto>().ReverseMap();
            conf.CreateMap<StockMovement, StockMovementDto>().ReverseMap();
            conf.CreateMap<User, UserSimpleDto>().ReverseMap();
        });

        mapper = config.CreateMapper();
    }

    public ProductDto ToDto(Product e) => mapper.Map<ProductDto>(e);
    public Pagination<ProductDto> ToDto(Pagination<Product> e) => mapper.Map<Pagination<ProductDto>>(e);
    public SellerDto ToDto(Seller e) => mapper.Map<SellerDto>(e);
    public Pagination<SellerDto> ToDto(Pagination<Seller> e) => mapper.Map<Pagination<SellerDto>>(e);

}
