using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Example.DAL;
using Example.DAL.Repositories;
using Example.Services.Context;
using Example.Services.Managers;
using Example.Services.Services;
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
            var container = CreateContainer();
            DependencyResolver.SetResolver(CreateAutofacDependencyResolver());

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
            app.UseAutofacWebApi(GlobalConfiguration.Configuration);

            GlobalConfiguration.Configuration.DependencyResolver = CreateAutofacWebApiDependencyResolver();

            return app;
        }

        public static AutofacDependencyResolver CreateAutofacDependencyResolver()
        {
            if (_autofacDependencyResolver != null)
                return _autofacDependencyResolver;

            _autofacDependencyResolver = new AutofacDependencyResolver(CreateContainer());

            return _autofacDependencyResolver;
        }

        public static AutofacWebApiDependencyResolver CreateAutofacWebApiDependencyResolver()
        {
            if (_autofacWebApiDependencyResolver != null)
                return _autofacWebApiDependencyResolver;

            _autofacWebApiDependencyResolver = new AutofacWebApiDependencyResolver(CreateContainer());

            return _autofacWebApiDependencyResolver;
        }

        private static IContainer CreateContainer()
        {
            if (_container != null)
                return _container;

            var builder = new ContainerBuilder();

            RegisterInfrastructure(builder);
            RegisterFrameworks(builder);
            RegisterTypes(builder);
            RegisterWebApi(builder, GlobalConfiguration.Configuration);

            _container = builder.Build();

            return _container;
        }

        private static void RegisterWebApi(ContainerBuilder builder, HttpConfiguration config)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);
        }

        private static void RegisterInfrastructure(ContainerBuilder builder)
        {
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();
            builder.RegisterModule<AutofacWebTypesModule>();
            builder.RegisterSource(new ViewRegistrationSource());
            builder.RegisterFilterProvider();
        }

        private static void RegisterFrameworks(ContainerBuilder builder)
        {
            // Identity
            builder.RegisterType<ExampleUserManager>().InstancePerRequest();
            builder.RegisterType<ExampleUserStore>().InstancePerRequest();
        }

        private static void RegisterTypes(ContainerBuilder builder)
        {
            // DbContext
            builder.RegisterType<ExampleDbContextProvider>().As<IDbContextProvider>().InstancePerRequest();

            // ApplicationContext
            builder.RegisterType<ExampleContext>().AsSelf().InstancePerRequest();

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