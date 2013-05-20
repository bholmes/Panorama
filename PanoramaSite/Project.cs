using System;

namespace PanoramaSite
{
    public class Project : PanoramBaseObject
    {
        public Project()
        {
            Lists = new System.Collections.Generic.List<List>();
        }

        public string Title { get; set; }
        public System.Collections.Generic.List<List> Lists { get; set; }
    }
}