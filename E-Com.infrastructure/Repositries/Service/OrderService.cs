﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using E_Com.Core.DTO;
using E_Com.Core.Entites.Order;
using E_Com.Core.interfaces;
using E_Com.Core.Services;
using E_Com.infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace E_Com.infrastructure.Repositries.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, AppDbContext context, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _mapper = mapper;
        }
        public async Task<Orders> CreateOrdersAsync(OrderDTO orderDTO, string BuyerEmail)
        {
            var basket = await _unitOfWork.CustomerBasket.GetBasketAsync(orderDTO.basketId);

            List<OrderItem> orderItems = new List<OrderItem>();

            foreach (var item in basket.basketItems)
            {
                var Product = await _unitOfWork.ProductRepositry.GetByIdAsync(item.Id);
                var orderItem = new OrderItem
                    (Product.Id, item.Image, Product.Name, item.Price, item.Quantity);
                orderItems.Add(orderItem);

            }
            var deliverMethod = await _context.DeliveryMethods.FirstOrDefaultAsync(m => m.Id == orderDTO.deliveryMethodId);

            var subTotal = orderItems.Sum(m => m.Price * m.Quantity);

            var ship = _mapper.Map<ShippingAddress>(orderDTO.shipAddress);


            

            var order = new
                Orders(BuyerEmail, subTotal, ship, deliverMethod, orderItems);

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public Task<IReadOnlyList<Orders>> GetAllOrdersForUserAsync(string BuyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Orders> GetOrderByIdAsync(int Id, string BuyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
