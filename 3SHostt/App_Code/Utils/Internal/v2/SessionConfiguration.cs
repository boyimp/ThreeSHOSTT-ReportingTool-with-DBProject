using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;
using System.Web;

namespace ThreeS.Report.v2.Configuration
{
    public class MyHttpControllerHandler : HttpControllerHandler, IRequiresSessionState
    {
        public MyHttpControllerHandler(RouteData routeData) : base(routeData)
        { }
    }

    public class MyHttpControllerRouteHandler : HttpControllerRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new MyHttpControllerHandler(requestContext.RouteData);
        }
    }
}//namespace
