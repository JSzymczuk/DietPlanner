using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DietPlanner.Entities
{
    public class AppUser : IdentityUser
    {
        public int Weight { get; set; }

        public int Heigth { get; set; }

        public int BirthYear { get; set; }

        //public string Avatar { get; set; }

        public virtual ICollection<Rating> Votes { get; set; }          // 1 do wielu
        public virtual ICollection<Comment> Comments { get; set; }      // 1 do wielu
        public virtual ICollection<Recipe> Recipes { get; set; }        // 1 do wielu
        public virtual ICollection<Meal> Meals { get; set; }            // 1 do wielu

        public virtual ICollection<Recipe> Farvorites { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}
