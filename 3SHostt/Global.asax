<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Http" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="ThreeS.Report.v2.Configuration" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        //string JQueryVer = "1.7.1";
        //ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
        //{
        //    Path = "~/Scripts/jquery-" + JQueryVer + ".min.js",
        //    DebugPath = "~/Scripts/jquery-" + JQueryVer + ".js",
        //    CdnPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-" + JQueryVer + ".min.js",
        //    CdnDebugPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-" + JQueryVer + ".js",
        //    CdnSupportsSecureConnection = true,
        //    LoadSuccessExpression = "window.jQuery"
        //});
        RouteTable.Routes.Ignore("{resource}.axd/{*pathInfo}");
        var route1 = RouteTable.Routes.MapHttpRoute(
                               name: "DefaultApi2",
                               routeTemplate: "api/{controller}/{action}",
                               defaults: new { id = System.Web.Http.RouteParameter.Optional }
                            );
        route1.RouteHandler = new MyHttpControllerRouteHandler();

       var route2 = RouteTable.Routes.MapHttpRoute(
                                name: "DefaultApi",
                                routeTemplate: "api/{controller}/{id}",
                                defaults: new { id = System.Web.Http.RouteParameter.Optional }
                            );
        route2.RouteHandler = new MyHttpControllerRouteHandler();

        var route3 = RouteTable.Routes.MapHttpRoute(
                                name: "AttributeApi",
                                routeTemplate: "api/{controller}/{action}/{id}",
                                defaults: new { id = System.Web.Http.RouteParameter.Optional }
                            );
        route3.RouteHandler = new MyHttpControllerRouteHandler();

         var route4 = RouteTable.Routes.MapHttpRoute(
                                name: "AttributeApi2",
                                routeTemplate: "api/{controller}/{action}/{name}",
                                defaults: new { id = System.Web.Http.RouteParameter.Optional }
                            );
        route4.RouteHandler = new MyHttpControllerRouteHandler();

        // RouteTable.Routes.MapHttpRoute(
        //    name: "Default",
        //    routeTemplate: "{controller}/{action}/{id}",
        //    defaults: new { controller = "UI", action = "Index", id = System.Web.Http.RouteParameter.Optional }
        //);

        RouteTable.Routes.MapHttpRoute(
                 name: "Default",
                 routeTemplate: "{controller}/{action}/{id}",
                 defaults: new { id = System.Web.Http.RouteParameter.Optional }
             );


        GlobalConfiguration.Configuration.EnsureInitialized();

    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }//End func

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        Exception exception = Server.GetLastError().GetBaseException();
        NLog.LogManager.GetCurrentClassLogger().Error(exception);

        //Response.Write("We're Sorry...");
        //Response.Write("An error has occured on the page you were requesting.  Your System Administrator has been notified <BR>");
        //Response.Write("Url: " + Request.Url.ToString() + "<BR>");
        //Response.Write("Err: " + exception.Message  + "<BR>");

    }//Error func

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started

    }//func

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends.
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer
        // or SQLServer, the event is not raised.

    }//func

</script>
