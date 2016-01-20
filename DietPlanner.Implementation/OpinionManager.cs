using DietPlanner.Contract;
using DietPlanner.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietPlanner.Implementation
{
    public class OpinionManager : DisposableManager, IOpinionManager
    {
        public IQueryable<Rating> Votes
        {
            get { return Context.Ratings; }
        }

        public IQueryable<Comment> Comments 
        {
            get { return Context.Comments; }
        }

        public OpinionManager(DietPlannerDbContext context) : base(context) { }

        public void CreateComment(Comment comment)
        {
            if (string.IsNullOrEmpty(comment.UserId) || string.IsNullOrEmpty(comment.Content)) { return; }
            comment.User = Context.Users.First(u => u.Id == comment.UserId);
            if (comment.User == null) { return; }
            comment.Added = DateTime.Now;   
            Context.Comments.Add(comment);
        }

        public void DeleteComment(Guid id)
        {
            Comment comment = Context.Comments.Find(id);
            Context.Comments.Remove(comment);
        }

        public void Rate(string userId, Guid recipeId, int rate)
        {
            Recipe recipe = Context.Recipes.First(r => r.Id == recipeId);
            if (recipe == null) { return; }
            int votes = recipe.Votes.Count;
            recipe.Rating = (recipe.Rating * votes + rate) / (votes + 1);
            Context.Ratings.Add(new Rating { UserId = userId, RecipeId = recipeId, Stars = rate });
            Context.Entry(recipe).State = EntityState.Modified;
        }

        public bool IsRated(string userId, Guid recipeId)
        {
            return Votes.Any(v => v.UserId == userId && v.RecipeId == recipeId);
        }
    }
}
