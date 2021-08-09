using System;
using System.Collections.Generic;
using System.Text;

namespace TirntirobSharp
{
    class PageManager
    {
        public static Response GetResponse(Request rq)
        {
            var r = new Response();
            if (rq.Method == "GET")
            {
                r.HTTPv = rq.HTTPv;
                r.Code = 200;
                r.ContentType = "text/html";
                r.Result = Encoding.UTF8.GetBytes("<a>Hello, world!</a><br><a>Request info:</a><br><a>HTTPv: "+rq.HTTPv+"</a><br><a>Method: "+rq.Method+"</a><br><a>Request uri: "+rq.URI+"</a><br><a>Remote IP: "+rq.RemoteIP+"</a>");
            }else r.ForceClose = true;
            return r;
        }
    }
}
