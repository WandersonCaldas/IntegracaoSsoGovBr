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
            //Random random = new Random();
            //string state = random.Next().ToString("X");
            //string nonce = random.Next().ToString("X");
            //string scope = "openid+(email/phone)+profile+govbr_empresa+govbr_confiabilidades";

            //var client = new RestClient(ConfigurationManager.AppSettings["UrlBase"].ToString() + "authorize");
            //var request = new RestRequest();
            //request.AddQueryParameter("response_type", "code");
            //request.AddQueryParameter("client_id", _clientId);
            //request.AddQueryParameter("scope", scope);
            //request.AddQueryParameter("redirect_uri", "http://localhost:12224/Autenticate");
            //request.AddQueryParameter("nonce", nonce);
            //request.AddQueryParameter("state", state);
            //var response = client.ExecuteGetAsync(request).Result;



            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}