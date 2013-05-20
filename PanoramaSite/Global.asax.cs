using Funq;
using MongoDB.Driver;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        }
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