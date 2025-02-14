using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NguyenQuocViet_2122110285.Context;
namespace NguyenQuocViet_2122110285.Models
{
    public class HomeModel
    {
        public List<Category> ListCategory { get; set; }
        public List<Product> ListProduct { get; set; }
    }
}