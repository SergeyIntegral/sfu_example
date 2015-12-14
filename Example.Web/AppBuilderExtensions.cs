using System.Data.Entity;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Example.DAL;
using Example.DAL.Entities;
using Example.DAL.Repositories;
using Example.Services.Context;
using Example.Services.Managers;
using Example.Services.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;

namespace Example.Web
{
    public static class AppBuilderExtensions
    {
        private static IContainer _container;
        private static AutofacDependencyResolver _autofacDependencyResolver;
        private static AutofacWebApiDependencyResolver _autofacWebApiDependencyResolver;

        public static IAppBuilder UseContainer(this IAppBuilder app)
        {
            var container = CreateContainer(app);
            DependencyResolver.SetResolver(CreateAutofacDependencyResolver(app));

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
            app.UseAutofacWebApi(GlobalConfiguration.Configuration);

            GlobalConfiguration.Configuration.DependencyResolver = CreateAutofacWebApiDependencyResolver(app);

            return app;
        }

        public static AutofacDependencyResolver CreateAutofacDependencyResolver(IAppBuilder app)
        {
            if (_autofacDependencyResolver != null)
                return _autofacDependencyResolver;

            _autofacDependencyResolver = new AutofacDependencyResolver(CreateContainer(app));

            return _autofacDependencyResolver;
        }

        public static AutofacWebApiDependencyResolver CreateAutofacWebApiDependencyResolver(IAppBuilder app)
        {
            if (_autofacWebApiDependencyResolver != null)
                return _autofacWebApiDependencyResolver;

            _autofacWebApiDependencyResolver = new AutofacWebApiDependencyResolver(CreateContainer(app));

            return _autofacWebApiDependencyResolver;
        }

        private static IContainer CreateContainer(IAppBuilder app)
        {
            if (_container != null)
                return _container;

            var builder = new ContainerBuilder();

            RegisterContexts(app, builder);
            RegisterFrameworks(app, builder);
            RegisterTypes(app, builder);
            RegisterInfrastructure(app, builder);
            RegisterWebApi(app, builder, GlobalConfiguration.Configuration);

            _container = builder.Build();

            return _container;
        }

        private static void RegisterWebApi(IAppBuilder app, ContainerBuilder builder, HttpConfiguration config)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);
        }

        private static void RegisterInfrastructure(IAppBuilder app, ContainerBuilder builder)
        {
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();
            builder.RegisterModule<AutofacWebTypesModule>();
            builder.RegisterSource(new ViewRegistrationSource());
            builder.RegisterFilterProvider();
        }

        private static void RegisterContexts(IAppBuilder app, ContainerBuilder builder)
        {
            // DbContext
            builder.RegisterType<ExampleDbContextProvider>().As<IDbContextProvider>().InstancePerRequest();

            // ApplicationContext
            builder.RegisterType<ExampleContext>().AsSelf().InstancePerRequest();
        }

        private static void RegisterFrameworks(IAppBuilder app, ContainerBuilder builder)
        {
            // Identity
            builder.RegisterType<ExampleUserStore>().As<IUserStore<ExampleUser>>()/*.WithParameter(dbContextParameter)*/.InstancePerRequest();
            builder.RegisterType<ExampleUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ExampleSignInManager>().AsSelf().InstancePerRequest();
            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register<IDataProtectionProvider>(c => app.GetDataProtectionProvider()).InstancePerRequest();
        }

        private static void RegisterTypes(IAppBuilder app, ContainerBuilder builder)
        {
            // Repositories
            builder.RegisterType<BinaryDataRepository>().As<IBinaryDataRepository>();
            builder.RegisterType<TopicRepository>().As<ITopicRepository>();
            builder.RegisterType<MessageRepository>().As<IMessageRepository>();
            builder.RegisterType<SectionRepository>().As<ISectionRepository>();

            // Services
            builder.RegisterType<BinaryDataService>().As<IBinaryDataService>();
            builder.RegisterType<TopicService>().As<ITopicService>();
            builder.RegisterType<MessageService>().As<IMessageService>();
            builder.RegisterType<SectionService>().As<ISectionService>();
        }
    }
}