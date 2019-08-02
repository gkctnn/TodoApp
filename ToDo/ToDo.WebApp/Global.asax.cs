using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ToDo.Bussiness;
using ToDo.Common;
using ToDo.Core.DataAccess;
using ToDo.WebApp.Init;
using ToDo.WebApp.Scheduler;
using ToDo.Core.Repositories;
using ToDo.Entities;
using ToDo.DataAccess.Repositories;
using System.Configuration;
using System.Data.SqlClient;

namespace ToDo.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //
            

            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());


            builder.RegisterGeneric(typeof(ToDoRepository<>)).AsSelf().As(typeof(IDataAccess<>)).InstancePerRequest();
            builder.RegisterType<DatabaseContext>().InstancePerRequest();

            builder.RegisterType<CategoryManager>().As<ICategoryManager>().InstancePerLifetimeScope();
            builder.RegisterType<NoteManager>().As<INoteManager>().InstancePerLifetimeScope();
            builder.RegisterType<TodoUserManager>().As<ITodoUserManager>().InstancePerLifetimeScope();

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            SqlDependency.Start(ConfigurationManager.ConnectionStrings["DatabaseContext"].ConnectionString); AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);


            App.Common = new WebCommon();

            SchedulerWrapper.Start();
        }
        protected void Application_End()
        {
            SqlDependency.Stop(ConfigurationManager.ConnectionStrings["DatabaseContext"].ConnectionString);
        }
    }
}
