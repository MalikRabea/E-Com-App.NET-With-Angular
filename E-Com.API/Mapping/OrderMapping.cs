using AutoMapper;
using E_Com.Core.Entites.Order;
using static E_Com.Core.DTO.OrderDTO;

namespace E_Com.API.Mapping
{
    public class OrderMapping:Profile
    {
        public OrderMapping()
        {
            CreateMap<ShippingAddress,ShipAddressDTO>().ReverseMap();
        }
    }
}
