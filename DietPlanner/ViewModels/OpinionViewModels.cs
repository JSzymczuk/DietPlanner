using DietPlanner.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DietPlanner.ViewModels
{
    public class AddCommentModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid RecipeId { get; set; }
    }

    public class CommentModel
    {
        [Key]
        public Guid Id { get; set; }
        public AppUserInfo AppUserInfo { get; set; }
        public DateTime Added { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class CommentsViewModel
    {
        public ICollection<CommentModel> Comments { get; set; }
        public bool UserIsAdmin { get; set; }
        public string UserId { get; set; }
        public Guid RecipeId { get; set; }
    }
}