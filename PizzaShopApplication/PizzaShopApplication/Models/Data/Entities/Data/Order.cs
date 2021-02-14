﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaShopApplication.Models.Data.Entities.Data
{
    // Данный класс предоставляет таблицу для заказов в БД
    // с полями, соответствующими определенным свойствам.
    public class Order
    {
        // Номер заказа.
        [Key]
        public int Id { get; set; }
        // Имя заказчика.
        [Required(ErrorMessage = "Пожалуйста, укажите ваше имя")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Имя должно быть в пределах от 3 до 30 символов")]
        public string ClientName { get; set; }
        // Номер телефона заказчика.
        [Required(ErrorMessage = "Пожалуйста, укажите ваш номер телефона")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Некорректный номер телефона")]
        public string Phone { get; set; }
        // Почта заказчика.
        [StringLength(50, MinimumLength = 7, ErrorMessage = "Адрес почты должен быть в пределах от 7 до 50 символов")]
        public string Email { get; set; }
        // Улица, указанная заказчиком.
        [Required(ErrorMessage = "Пожалуйста, укажите улицу для доставки.")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина назавания улицы от 1 до 50 символов!")]
        public string Street { get; set; }
        // Номер дома, указанный заказчиком.
        [Required(ErrorMessage = "Пожалуйста, укажите номер дома для доставки")]
        [StringLength(5, MinimumLength = 1, ErrorMessage ="Номер дома должен быть в пределах от 1 до 5 символов!")]
        public string Home { get; set; }
        // Номер квартиры, указанная заказчиком
        [StringLength(5, MinimumLength = 1, ErrorMessage = "Номер квартиры должен быть в пределах от 1 до 5 символов!")]
        public string Apartment { get; set; }
        // Номер этажа заказчика.
        [Range(1, 3, ErrorMessage = "Максимальная длина для номера подъезда составляет 3 символа")]
        public int FloorNubmer { get; set; }
        // Номер подъезда заказчика.
        [Range(1, 3, ErrorMessage = "Максимальная длина для номера подъезда составляет 2 символа")]
        public int EntranceNubmer { get; set; }
        // Комментарий к заказу.
        [MaxLength(200, ErrorMessage = "Слишком длинный комментарий. Максимум - 200 символов")]
        public string Comment { get; set; }
        // Id корзины пользователя на момент заказа.
        //public string ShoppingCartId { get; set; }
        // Дата и время заказа.
        public DateTime OrderDateTime { get; set; }

        // Guid пользовательской корзины(кука в браузере).
        public Guid UserCartForeignKey { get; set; }

        // Связь с таблицей OrderStatus.
        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }

    }
}