using System;
using System.Net;
using Funq;

namespace ServiceStack.Hello
{
    /// <summary>
    /// Define your ServiceStack web service request (i.e. the Request DTO).
    /// </summary>    
    public class Hello
    {
        public string Name { get; set; }
    }

    /// <summary>
    /// Define your ServiceStack web service response (i.e. Response DTO).
    /// </summary>
    public class HelloResponse
    {
        public string Result { get; set; }
    }

    /// <summary>
    /// Create your ServiceStack web service implementation.
    /// </summary>
    public class HelloService : IService
    {
        public object Any(Hello request)
        {
            //Looks strange when the name is null so we replace with a generic name.
            var name = request.Name ?? "John Doe";
            return new HelloResponse { Result = "Hello, " + name };
        }
    }

    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// Create your ServiceStack web service application with a singleton AppHost.
        /// </summary>        
        public class AppHost : AppHostBase
        {
            /// <summary>
            /// Initializes a new instance of your ServiceStack application, with the specified name and assembly containing the services.
            /// </summary>
            public AppHost() : base("Hello Web Services", typeof(HelloService).Assembly) { }

            /// <summary>
            /// Configure the container with the necessary routes for your ServiceStack application.
            /// </summary>
            /// <param name="container">The built-in IoC used with ServiceStack.</param>
            public override void Configure(Container container)
            {
                //Register user-defined REST-ful urls. You can access the service at the url similar to the following.
                Routes
                  .Add<Hello>("/hello")
                  .Add<Hello>("/hello/{Name}");
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            //Initialize your application
            new AppHost().Init();
        }
    }

    [Route("/mythz_blog")]
    public class MythzBlog
    {
        public int? P { get; set; }
    }

    [Route("/docs/{Path*}")]
    public class OldDocs
    {
        public string Path { get; set; }
    }

    public class RedirectService : Service
    {
        public object Any(MythzBlog request)
        {
            var url = request.P == null
                ? "https://github.com/ServiceStackV3/mythz_blog"
                : "https://github.com/ServiceStackV3/mythz_blog/blob/master/pages/{0}.md".Fmt(request.P);

            return HttpResult.Redirect(url, HttpStatusCode.MovedPermanently);
        }

        public object Any(OldDocs request)
        {
            var url = "http://docs.servicestack.net/{0}".Fmt(request.Path);

            return HttpResult.Redirect(url, HttpStatusCode.MovedPermanently);
        }
    }
}
