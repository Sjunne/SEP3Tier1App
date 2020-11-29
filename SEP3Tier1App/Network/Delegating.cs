using System;
using System.Collections.Generic;
using WebApplication.Data;

namespace SEP3Tier1App.Network
{
    public class Delegating
    {
        public Action<Request> fromNetwork { get; set; }
        public Action<IList<string>> ImagesFromNetwork { get; set; }
        
        
    }
}