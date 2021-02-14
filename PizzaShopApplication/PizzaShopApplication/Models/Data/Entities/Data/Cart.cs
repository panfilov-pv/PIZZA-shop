﻿using PizzaShopApplication.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaShopApplication.Models.Data.Entities.Data
{
    public class Cart
    {
        [Key]
        public Guid ItemId { get; set; }
        // Id пользователя, связанного с приобретаемым элементом.
        // Будет храниться как переменная сеанса.
        public Guid UserId { get; set; }
        public int Quantity { get; set; }
        public DateTime DateCreated { get; set; }
        // Идентификатор продукта, находящегося в корзине.
        public int PizzaId { get; set; }
        public Pizza Pizza{ get; set; }
    }
}