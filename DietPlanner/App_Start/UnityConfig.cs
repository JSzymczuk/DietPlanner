using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Data.Entity;
using DietPlanner.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using DietPlanner.Controllers;
using Microsoft.Owin.Security;
using System.Web;
using DietPlanner.Implementation;
using DietPlanner.Contract;

namespace DietPlanner.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<DbContext, DietPlannerDbContext>(new PerRequestLifetimeManager());
            container.RegisterType<IUserStore<AppUser>, UserStore<AppUser>>(new PerRequestLifetimeManager());

            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<NotificationController>(new InjectionConstructor());

            container.RegisterType<ApplicationUserManager>(new PerRequestLifetimeManager());
            container.RegisterType<ApplicationSignInManager>(new PerRequestLifetimeManager());
            container.RegisterType<IAuthenticationManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));

            container.RegisterType<IProductManager, ProductManager>(new PerRequestLifetimeManager());
            container.RegisterType<IRecipeManager, RecipeManager>(new PerRequestLifetimeManager());
            container.RegisterType<IMeasureManager, MeasureManager>(new PerRequestLifetimeManager());
            container.RegisterType<IRecipeManager, RecipeManager>(new PerRequestLifetimeManager());
        }
    }
}
