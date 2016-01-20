using DietPlanner.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietPlanner.Contract
{
    public interface IOpinionManager : IDisposable
    {
        IQueryable<Rating> Votes { get; }
        IQueryable<Comment> Comments { get; }
        void CreateComment(Comment comment);
        void DeleteComment(Guid id);
        void Rate(string userId, Guid recipeId, int rate);
        bool IsRated(string userId, Guid recipeId);
        void Save();
    }
}
