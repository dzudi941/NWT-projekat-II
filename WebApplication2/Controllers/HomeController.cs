using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult jQuery()
        {
            var fileContents = System.Web.HttpContext.Current.Request.MapPath("~/Views/WebApiClient/index.html");
            //var response = new HttpResponseMessage();
            //response.Content = new StringContent(fileContents);
            //response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            //return response;
            //return Content(fileContents);
            return File(Server.MapPath("/Views/WebApiClient/") + "index.html", "text/html");
        }
    }
}