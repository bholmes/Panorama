using Funq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using MongoDB.Driver;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace PanoramaSite
{
    [Route("/projectcontents/{ProjectId}")]
    public class ProjectContents
    {
        public int ProjectId { get; set; }
    }

    [Route("/movecard")]
    public class MoveCardArgs
    {
        public int Project { get; set; }
        public int List { get; set; }
        public int Card { get; set; }
        public int Index { get; set; }
    }

    [Route("/movelist")]
    public class MoveListArgs
    {
        public int Project { get; set; }
        public int List { get; set; }
        public int Index { get; set; }
    }

    public class ProjectService : Service
    {
        public object Get(ProjectContents projectContents)
        {
            MongoDBProjectDriver driver = new MongoDBProjectDriver();

            return driver.GetProject(new Project { Id = projectContents.ProjectId });
        }

        public void Put(MoveCardArgs moveCardArgs)
        {
            MongoDBProjectDriver driver = new MongoDBProjectDriver();

            driver.MoveCardToList(new Project { Id = moveCardArgs.Project }, 
                new List { Id = moveCardArgs.List }, new Card { Id = moveCardArgs.Card}, moveCardArgs.Index);

            var hub = GlobalHost.ConnectionManager.GetHubContext("projecthub");
            hub.Clients.All.Invoke("refresh_project", moveCardArgs.Project);
        }

        public void Put(MoveListArgs moveListArgs)
        {
            MongoDBProjectDriver driver = new MongoDBProjectDriver();

            driver.MoveList (new Project { Id = moveListArgs.Project },
                new List { Id = moveListArgs.List }, moveListArgs.Index);

            var hub = GlobalHost.ConnectionManager.GetHubContext("projecthub");
            hub.Clients.All.Invoke("refresh_project", moveListArgs.Project);
        }
    }

    [HubName("projecthub")]
    public class ProjectHub : Hub
    {

    }

    public class Global : System.Web.HttpApplication
    {
        public class PanoramaAppHost : AppHostBase
        {
            //Tell Service Stack the name of your application and where to find your web services
            public PanoramaAppHost() : base("Panorama Web Services", typeof(ProjectService).Assembly) { }

            public override void Configure(Container container)
            {

            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            new PanoramaAppHost().Init();
            RouteTable.Routes.MapHubs();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}