using NguyenQuocViet_2122110285.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NguyenQuocViet_2122110285.Models
{
    public class CartItemModel
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal TotalPrice 
        { 
            get { return Price * Quantity; }
        }
    }
}