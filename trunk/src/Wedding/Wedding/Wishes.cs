using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wedding
{
    public class Wishes
    {
        public int WishesId { get; set; }
        public string Name { get; set; }
        public string fbid { get; set; }
        public string message { get; set; }
        public int siteId { get; set; }

    }
}