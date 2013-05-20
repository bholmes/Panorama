using System;

namespace PanoramaSite
{
    public class List : PanoramBaseObject
    {
        public List()
        {
            Cards = new System.Collections.Generic.List<Card>();
        }
        public string Title { get; set; }
        public System.Collections.Generic.List<Card> Cards { get; set; }
    }
}