using Funq;
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
    [Route("/projectcontents")]
    public class ProjectContents
    {
        
    }

    public class Project
    {
        public List[] Lists { get; set; }
    }

    public class List
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Card[] Cards { get; set; }
    }

    public class Card
    {
        public int Id { get;set; }
        public string Title { get;set; }
    }

    public class ProjectService : Service
    {
        public object Get(ProjectContents projectContents)
        {
            return new Project { 
                Lists = new List [] {
                    new List {
                        Id=1, Title="Ideas", Cards= new Card [] {
                            new Card {Id=3, Title="Drink"},
                            new Card {Id=5, Title="Play"}
                        }
                    },
                    new List {
                        Id=2, Title="Todo", Cards=new Card [] {
                            new Card {Id=1, Title="Eat"},
                            new Card {Id=2, Title="Sleep"},
                            new Card {Id=4, Title="Work"}
                        }
                    },
                    new List{
                        Id=3, Title="In Progress", Cards=new Card [] {
                        }
                    },
                    new List{
                        Id=4, Title="Completed", Cards=new Card [] {
                        }
                    }
                } 
            };
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