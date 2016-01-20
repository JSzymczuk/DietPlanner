using AutoMapper;
using DietPlanner.Contract;
using DietPlanner.Entities;
using DietPlanner.Helpers;
using DietPlanner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DietPlanner.Controllers
{
    public class OpinionController : Controller
    {
        private IOpinionManager opinionManager;

        public IOpinionManager OpinionManager
        {
            get { return opinionManager; }
            set { opinionManager = value; }
        }

        public OpinionController() { }

        public OpinionController(IOpinionManager opinionManager)
        {
            OpinionManager = opinionManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Rate(Guid id, int rate)
        {
            if (rate > 0 && rate < 6)
            {
                string userId = AccountHelper.GetLoggedUserId();
                if (!OpinionManager.IsRated(userId, id))
                {
                    OpinionManager.Rate(userId, id, rate);
                    OpinionManager.Save();
                    return RedirectToAction("Rate", "Recipe", new { success = true });
                }
                else
                {
                    return RedirectToAction("Rate", "Recipe",
                        new { success = false, error = "Już oddałeś głos na ten przepis." });
                }
            }
            else
            {
                return RedirectToAction("Rate", "Recipe", new { success = false, error = "Wystąpił błąd." });
            }
        }

        [HttpPost]
        public PartialViewResult AddComment(AddCommentModel comment)
        {
            OpinionManager.CreateComment(Mapper.Map<AddCommentModel, Comment>(comment));
            OpinionManager.Save();
            return RecipeComments(comment.RecipeId);
        }

        public PartialViewResult DeleteComment(Guid commentId, Guid? recipeId)
        {
            OpinionManager.DeleteComment(commentId);
            OpinionManager.Save();
            return RecipeComments(recipeId);
        }

        public PartialViewResult RecipeComments(Guid? recipeId)
        {
            if (recipeId.HasValue)
            {
                CommentsViewModel model = new CommentsViewModel
                {
                    Comments = Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentModel>>(
                        OpinionManager.Comments.Where(c => c.RecipeId == recipeId)).OrderByDescending(c =>
                        c.Added).ToList(),
                    RecipeId = recipeId.Value,
                    UserId = AccountHelper.GetLoggedUserId(),
                    UserIsAdmin = AccountHelper.UserInRole("Administrator")

                };
                ModelState.Remove("Title");
                ModelState.Remove("Content");
                return PartialView("_Comments", model);
            }
            else
            {
                return PartialView("_Comments");
            }
        }
    }
}