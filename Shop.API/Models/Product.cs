﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shop.API.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Sku { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public int CategoryId { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }

    }
}
