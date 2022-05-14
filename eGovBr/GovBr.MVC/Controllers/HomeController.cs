using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Configuration;
using RestSharp;

namespace GovBr.MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {          
            return View();
        }        
    }
}