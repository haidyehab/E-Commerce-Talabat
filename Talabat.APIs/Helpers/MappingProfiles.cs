using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.OrdersAggregate;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles()
        {
            //d => ProductToReturnDto
            //S => Product
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductBrand , O => O.MapFrom(S => S.ProductBrand.Name))
                .ForMember(d => d.ProductType , O => O.MapFrom(S => S.ProductType.Name))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());

            //Address
            CreateMap<Talabat.Core.Entities.Identity.Address, AddressDto>().ReverseMap();

            //customerBasket
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            //Aggregate Address
            CreateMap<AddressDto, Talabat.Core.Entities.OrdersAggregate.Address>();

            //order
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));
            //orderItem 
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, O => O.MapFrom(S => S.Product.ProductId))
                .ForMember(d => d.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                .ForMember(d => d.PictureUrl, O => O.MapFrom(S => S.Product.PictureUrl))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());



        }
    }
}
