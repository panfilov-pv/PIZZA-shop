﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PizzaShopApplication.Models.Data.Context;
using PizzaShopApplication.Models.Data.Domain.Interfaces;
using PizzaShopApplication.Models.Data.Entities.Data;
using PizzaShopApplication.Models.Filtration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaShopApplication.Models.Data.Domain
{
    /// <summary>
    /// This class represents the ability to use 
    /// CRUD opertaions in "Orders" table in DB.
    /// </summary>
    public class OrderRepository : IOrder
    {
        private readonly ApplicationDataContext _dbContext;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ShoppingCartRepository _cartRepository;
        public OrderRepository(ApplicationDataContext dbContext,
            IHttpContextAccessor httpContext, ShoppingCartRepository cartRepository)
        {
            _dbContext = dbContext;
            _httpContext = httpContext;
            _cartRepository = cartRepository;
        }
        // Получает список заказов с встроенной системой фильтрации.
        public List<Order> GetOrdersWithFiltration(int? orderStatusId, int? orderId, DateTime date)
        {
            var orders = _dbContext.Orders.Include(o => o.OrderStatus).ToListAsync().Result;
            if (orderStatusId != null && orderStatusId != 0)
            {
                orders = orders.Where(o => o.OrderStatusId == orderStatusId).ToList();
            }
            if (orderId != null && orderId != 0)
            {
                orders = orders.Where(o => o.Id == orderId).ToList();
            }
            // В случае, если поле во View будет не заполнено,
            // то значение даты будет эквивалентно DateTime.MinValue.
            if (date != null && date != DateTime.MinValue)
            {
                orders = orders.Where(o => o.OrderDateTime.Date == date.Date).ToList();
            }
            return orders;
        }
        // Добавляет заказ в базу данных.
        public async Task AddOrderToDBAsync(Order order)
        {
            // Устанавливает для заказа статус - в процессе доставки.
            order.OrderStatusId = 3;
            order.OrderDateTime = DateTime.UtcNow;
            var cartGuid = _httpContext.HttpContext.Request.Cookies["CartId"];
            order.UserCartForeignKey = Guid.Parse(cartGuid);
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// Gets a order contains in database.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<Order> GetOrderFromDBAsync(Order order)
        {
            order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.OrderDateTime == order.OrderDateTime && o.Phone == order.Phone);
            return order;
        }
        // Получает заказ, сохраненный в базе данных по Id.
        public async Task<Order> GetOrderFromDBAsync(int orderId)
        {
            return await _dbContext.Orders.Include(o => o.OrderStatus).FirstOrDefaultAsync(o => o.Id == orderId);
        }
        // Находит в базе данных все продукты(пиццы) по id заказа.
        public IEnumerable<Cart> GetConcreteCartFromOrder(string cartId)
        {
            var conreteCart = _cartRepository.GetConcreteCartAsync(cartId);
            return conreteCart;
        }
        public IEnumerable<OrderStatus> GetOrderStatuses()
        {
            return _dbContext.OrderStatuses;
        }
        public async Task ChangeOrderStatusAsync(int orderId, int orderStatusId)
        {
            var orderToChange = _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId).Result;
            orderToChange.OrderStatusId = orderStatusId;
            await _dbContext.SaveChangesAsync();
        }
    }
}
