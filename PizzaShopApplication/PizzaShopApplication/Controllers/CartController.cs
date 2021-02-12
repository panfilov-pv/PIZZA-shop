﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzaShopApplication.Models.Data.Domain;
using PizzaShopApplication.Models.Data;
using PizzaShopApplication.Models.Secondary.Entities;

namespace PizzaShopApplication.Controllers
{
    public class CartController : Controller
    {
        private readonly ShoppingCartRepository cart;
        public CartController(ShoppingCartRepository cart)
        {
            this.cart = cart;
        }
        public async Task<IActionResult> AddItemToCart(int itemId, string itemName)
        {
            await cart.AddToCartAsync(itemId);
            ViewBag.Name = itemName;
            return View();
        }
        public async Task<IActionResult> AddItemToCartChangeEvent(int itemId)
        {
            await cart.AddToCartAsync(itemId);
            return RedirectPermanent("~/Cart/GetUserCartInfo");
        }
        public async Task<IActionResult> DeleteItemFromCart(int itemId)
        {
            await cart.DeleteFromCartAsync(itemId);
            return RedirectPermanent("~/Cart/GetUserCartInfo");
        }
        public async Task<IActionResult> GetUserCartInfo()
        {
            var cartItems = await cart.GetCartItemsAsync();
            decimal totalSum = 0;
            foreach (var cartItem in cartItems)
            {
                var counter = cartItem.PizzaCount;
                while (counter > 0)
                {
                    totalSum += cartItem.PizzaPrice;
                    counter--;
                }
            }
            ViewBag.TotalSum = totalSum;
            return View(cartItems);
        }
    }
}
