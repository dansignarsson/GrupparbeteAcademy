using GroupJoinMVC.Models.Data;
using GroupJoinMVC.Models.Entities;
using GroupJoinMVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupJoinMVC.Models
{
    public class ProductService
    {
        public ProductService(GroupJoinDBContext context)
        {
            this.context = context;
        }
        readonly GroupJoinDBContext context;


        internal void AddItemToDB(ProductsCreateVM product)
        {
            Products x = new Products();

            x.Brand = product.Brand;
            x.Description = product.Description;
            x.ImgUrl = product.ImgUrl;
            x.Price = product.Price;
            x.Name = product.Name;
            context.Products.Add(x);
            context.SaveChanges();

            var productId = GetProductID(x);
            foreach (var item in product.SelectedTemperValues)
            {
                Products2Stores p2s = new Products2Stores()
                {
                    ProductId = productId,
                    StoreId = item
                };
                context.Products2Stores.Add(p2s);
            }
            context.SaveChanges();

        }

        internal ProductsCreateVM GetAllStoresFromDBForPCreateVM()
        {

            Stores[] x = GetAllStoresFromDB();
            var viewModel = new ProductsCreateVM();

            viewModel.TemperItems = new SelectListItem[x.Length];

            for (int i = 0; i < x.Length; i++)
            {
                viewModel.TemperItems[i] = new SelectListItem
                {
                    Value = $"{x[i].Id.ToString()}",
                    Text = $"{x[i].StoreName}"
                };

            }

            return viewModel;
        }



        public Stores[] GetAllStoresFromDB()
        {
            return context.Stores
                            .OrderBy(p => p.StoreName)
                            .Select(p => new Stores
                            {
                                 Id = p.Id,
                                 StoreName = p.StoreName
                            })
                            .ToArray();
        }

        private int GetProductID(Products x)
        {
            return context.Products
              .Where(p => p.Name.Contains(x.Name) && p.Brand.Contains(x.Brand) && p.Description.Contains(x.Description))
              .Select(p => p.Id)
              .FirstOrDefault();
        }

        private int GetStoreIDByName(string store)
        {
            return context.Stores
              .Where(p => p.StoreName.Contains(store))
              .Select(p => p.Id)
              .FirstOrDefault();
        }

        public List<ProductSearchBarResultVM> GetSearchValue(string s)
        {
            return CheckIfProductsContainsInputText(s);
        }

        public List<ProductSearchBarResultVM> GetProductsCategorizedByInputText(string s)
        {
            List<ProductSearchBarResultVM> searchResult = CheckIfProductsContainsInputText(s);
            List<string> everyWord = SplitStringsBySpace(searchResult);
            List<ProductSearchBarResultVM> finalsearchResult = CheckIfWordsContainsInputText(s, everyWord);

            return finalsearchResult;
        }

        public static List<string> SplitStringsBySpace(List<ProductSearchBarResultVM> searchResult)
        {
            List<string> everyWord = new List<string>();
            foreach (var item in searchResult)
            {
                everyWord.AddRange(item.Name.Split(' '));
            }

            return everyWord;
        }

        public List<ProductSearchBarResultVM> CheckIfProductsContainsInputText(string s)
        {
            return context.Products
                   .Where(p => p.Name.Contains(s))
                   .Select(p => new ProductSearchBarResultVM
                   {
                       Name = p.Name
                   }).ToList();
        }

        public static List<ProductSearchBarResultVM> CheckIfWordsContainsInputText(string s, List<string> everyWord)
        {
            List<ProductSearchBarResultVM> finalsearchResult = new List<ProductSearchBarResultVM>();
            bool addToList = false;
            foreach (var item in everyWord)
            {
                if (item.Contains(s))
                {
                    ProductSearchBarResultVM x = new ProductSearchBarResultVM();
                    x.Name = item;
                    foreach (var y in finalsearchResult)
                    {
                        addToList = y.Name == x.Name;
                        if (addToList)
                        {
                            break;
                        }
                    };

                    if (addToList == false)
                    {
                        finalsearchResult.Add(x);

                    }
                }
            }

            return finalsearchResult;
        }

        public List<ProductSearchTotalResultVM> GetSearchTotalResult(string s)
        {
            List<ProductSearchTotalResultVM> searchResult = context.Products

                   .Where(p => p.Name.Contains(s))
                   .Select(p => new ProductSearchTotalResultVM
                   {
                       Name = p.Name,
                       Id = p.Id,
                       Brand = p.Brand,
                       ImgUrl = p.ImgUrl,
                       Price = p.Price,
                       
                       //TotalComments = p.Rating.Count()

                   })
                   .OrderByDescending(o => o.Rating)
                   .ToList();

            return searchResult;
        }
        internal List<ProductSearchTotalResultVM> GetSearchTotalResult(GetSearchResult data)
        {
            try
            {
                List<ProductSearchTotalResultVM> searchResult = context.Products
                       .Where(p => p.Name.Contains(data.searchText) &&
                       (data.storeIds.Length == 0 || p.Products2Stores.Any(o => data.storeIds.Contains(o.StoreId))))
                       .Select(p => new ProductSearchTotalResultVM
                       {
                           Name = p.Name,
                           Id = p.Id,
                           Brand = p.Brand,
                           ImgUrl = p.ImgUrl,
                           Price = p.Price,
                           Rating = GetAVGRating(p.Rating),
                           TotalComments = p.Rating.Count()
                       })
                       .OrderByDescending(o => o.Rating)
                       .ToList();

                return searchResult;
            }
            catch
            {
                return null;
            }
        }

        public double GetAVGRating(ICollection<Rating> rating)
        {
            double total = 0.0;
            foreach (var item in rating)
            {
                total += (double)item.Rating1;
            }
            var count = rating.Count();
            var newtotal = total / count;
            return newtotal;
        }
        internal Products[] GetAllProductsFromDB()
        {
            return context.Products
                .OrderByDescending(p => p.Id)
                .Select(p => new Products
                {
                    Id = p.Id,
                    Name = p.Name,
                    Brand = p.Brand,
                    Description = p.Description,
                    ImgUrl = p.ImgUrl,
                    Price = p.Price,
                    Rating = p.Rating
                })
                .ToArray();
        }



        internal ProductDetailsVM GetDetailsByID(int id)
        {
            ProductDetailsVM productToReturn = context.Products
                    .Where(p => p.Id == id)
                   .Select(p => new ProductDetailsVM
                   {
                       Name = p.Name,
                       Description = p.Description,
                       Brand = p.Brand,
                       ImgUrl = p.ImgUrl,
                       Price = p.Price,
                       AVGRating = GetAVGRating(p.Rating),
                           Ratings = p.Rating.Select(r => new ProductDetailsRatingVM
                           {
                               FirstName = r.User.FirstName,
                               Comment = r.Comment,
                               Rating = r.Rating1.Value,
                               Id = r.Id
                           }).OrderByDescending(x => x.Id)
                           .ToArray()
                   })
                   .OrderByDescending(o => o.Ratings)
                   .Single();

            return productToReturn;
        }

    }
}
