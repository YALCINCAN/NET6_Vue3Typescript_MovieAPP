using Autofac;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthManager>().As<IAuthService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<TokenManager>().As<ITokenService>().InstancePerLifetimeScope();

            builder.RegisterType<RefreshTokenManager>().As<IRefreshTokenService>().InstancePerLifetimeScope();
            builder.RegisterType<EfRefreshTokenRepository>().As<IRefreshTokenRepository>().InstancePerLifetimeScope();

            builder.RegisterType<CategoryManager>().As<ICategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<EfCategoryRepository>().As<ICategoryRepository>().InstancePerLifetimeScope();

            builder.RegisterType<CommentManager>().As<ICommentService>().InstancePerLifetimeScope();
            builder.RegisterType<EfCommentRepository>().As<ICommentRepository>().InstancePerLifetimeScope();

            builder.RegisterType<ActorManager>().As<IActorService>().InstancePerLifetimeScope();
            builder.RegisterType<EfActorRepository>().As<IActorRepository>().InstancePerLifetimeScope();

            builder.RegisterType<MovieManager>().As<IMovieService>().InstancePerLifetimeScope();
            builder.RegisterType<EfMovieRepository>().As<IMovieRepository>().InstancePerLifetimeScope();

            builder.RegisterType<MovieImageManager>().As<IMovieImageService>().InstancePerLifetimeScope();
            builder.RegisterType<EfMovieImageRepository>().As<IMovieImageRepository>().InstancePerLifetimeScope();


            builder.RegisterType<FavoriteManager>().As<IFavoriteService>().InstancePerLifetimeScope();
            builder.RegisterType<EfFavoriteRepository>().As<IFavoriteRepository>().InstancePerLifetimeScope();

            

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic)
                .As<Profile>();

            builder.Register(c => new MapperConfiguration(cfg => {
                foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            })).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>()
                .CreateMapper(c.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                            .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                            {
                                Selector = new AspectInterceptorSelector()
                            }).SingleInstance().InstancePerDependency();
        }
    }
}
