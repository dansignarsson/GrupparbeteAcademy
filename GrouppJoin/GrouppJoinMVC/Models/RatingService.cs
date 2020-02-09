using GroupJoinMVC.Models.Data;
using GroupJoinMVC.Models.Entities;
using GroupJoinMVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupJoinMVC.Models
{
    public class RatingService
    {

        public RatingService(GroupJoinDBContext context)
        {
            this.context = context;
        }
        readonly GroupJoinDBContext context;

        internal RatingAddVM GetAllProductsForDropDownList()
        {
            Products[] x = GetAllProductsFromDB_ID_NAME();
            var viewModel = new RatingAddVM();

            viewModel.ListOfProducts = new SelectListItem[x.Length];

            for (int i = 0; i < x.Length; i++)
            {
                viewModel.ListOfProducts[i] = new SelectListItem
                {
                    Value = $"{x[i].Id.ToString()}",
                    Text = $"{x[i].Name}"
                };

            }

            return viewModel;
        }

        internal void AddRatingToDB(RatingAddVM ra)
        {

            Rating x = new Rating();
            x.ProductId = ra.SelectedProductValue;
            x.Rating1 = ra.Rating;
            x.Comment = ra.Comment;
            x.UserId = ra.UserId;


            context.Rating.Add(x);
            context.SaveChanges();

        }
        internal bool AddUserRatingToDB(RatingAddVM producttorate)
        {
            try
            {
                Rating r = new Rating();
                r.UserId = producttorate.UserId;
                r.ProductId = producttorate.ProductId;
                r.Rating1 = producttorate.Rating;
                r.Comment = producttorate.Comment;

                context.Rating.Add(r);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal bool DeleteRatingFromDB(RatingId data)
        {
            try
            {
                var x = context.Rating
                    .Where(i => i.Id == data.RatingID)
                    .SingleOrDefault();

                context.Rating.Remove(x);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }

        internal RatingAllUserRatingsVM GetUserRatings(UserId data)
        {
            var x = context.Rating
                     .Where(p => p.UserId == data.UserID)
                     .Select(p => new RatingAllUserRatingsVM
                     {

                         Ratings = context.Rating
                         .Where(q => q.UserId == data.UserID)
                         .Select(q => new UserRatings
                         {
                             RatingID = q.Id,
                             ProductID = q.ProductId,
                             AVGRating = q.Product.Rating.Average(o => o.Rating1).Value,
                             ImgUrl = q.Product.ImgUrl,
                             ProductName = q.Product.Name,
                             UserComment = q.Comment,
                             UserRating = q.Rating1.Value
                         })
                         .ToArray(),
                         FirstName = context.AspNetUsers
                             .Where(o => o.Id == data.UserID)
                             .Select(o => o.FirstName)
                             .FirstOrDefault(),
                         Email = context.AspNetUsers
                         .Where(o => o.Id == data.UserID)
                         .Select(o => o.Email)
                         .FirstOrDefault()
                     })/*.OrderBy(o => context.Rating
                .Select(k => k.Id))*/
                     .FirstOrDefault();

            return x;



        }

        internal Products[] GetAllProductsFromDB_ID_NAME()
        {
            return context.Products
                .OrderBy(p => p.Id)
                .Select(p => new Products
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToArray();
        }

    }
}
