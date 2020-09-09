﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingWebsiteMVC.Models;


namespace ShoppingWebsiteMVC.Controllers
{
    public class ProductController : Controller
    {
        
        // GET : Products by Category
        [Authorize]
        [HttpGet]
        public ActionResult Index(string category)
        {
            if(category==null)
            {
                // returns the category view(Index view)
                return View();
            }
            else
            {
                using(ProductContext db=new ProductContext())
                {
                    category = category.ToLower();
                    if (category == "all")
                    {
                        // return the View named Products with all products list
                        return View("Products",db.Products.ToList());
                    }
                    else
                    {
                        var products = db.Products.Where(p => p.CategoryName.Equals(category)).FirstOrDefault();
                        if (products != null)
                        {
                            // return the View named Products with the required category lists 
                            return View("Products",products);
                        }
                        else
                        {
                            ViewBag.Error = "Invalid Category";
                        }
                        // return the View named Products without any data 
                        return View("Products");
                    }
                }
            }
        }
        //Get Search product by productname
        public ActionResult Search(string ProductName)
        {
            List<Product> list = new List<Product>();
            using(ProductContext db=new ProductContext())
            {
                if(String.IsNullOrEmpty(ProductName))

                {
                    var query = from product in db.Products select new Product { ProductId = product.ProductId, ProductName = product.ProductName, CategoryName = product.CategoryName, Price = product.Price,Units=product.Units,Discount = product.Discount, SupplierName = product.SupplierName };
                    list = query.ToList();

                }
                else
                {
                    var query = from product in db.Products
                                where product.ProductName.ToLower().Contains(ProductName.ToLower())
                                select new Product { ProductId = product.ProductId, ProductName = product.ProductName, CategoryName = product.CategoryName, Price = product.Price, Units = product.Units, Discount = product.Discount, SupplierName = product.SupplierName };
                    list = query.ToList();

                }
                ViewBag.Title= "Search Result";
                return View("Products", list);
            }
        }
    }
}