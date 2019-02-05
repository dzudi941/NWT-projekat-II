using System.Web.Http;
using System.Web.Mvc;
using Unity;
using Unity.WebApi;
using WebApplication2.Models;
using WebApplication2.Repositories;

namespace WebApplication2
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterSingleton<IVideoClubContext, VideoClubContext>();
            container.RegisterType<IRepository<Actor>, ActorRepository>();
            container.RegisterType<IRepository<Country>, CountryRepository>();
            container.RegisterType<IRepository<Genre>, GenreRepository>();
            container.RegisterType<IRepository<Movie>, MovieRepository>();

            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}