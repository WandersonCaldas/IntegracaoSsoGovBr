using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace GovBr.MVC.Controllers
{
    public class LoginController : Controller
    {
        private string _clientId = ConfigurationManager.AppSettings["ClientId"];
        private string _clientSecret = ConfigurationManager.AppSettings["ClientSecret"];

        public ActionResult Index()
        {
            Random random = new Random();
            string state = random.Next().ToString("X");
            string nonce = random.Next().ToString("X");
            //string scope = "openid+(email/phone)+profile+govbr_empresa+govbr_confiabilidades";
            string scope = "openid+(email/phone)+profile+govbr_confiabilidades";
            string urlAuthorize = ConfigurationManager.AppSettings["UrlAuthorize"].ToString();
            string redirectUrl = Url.Encode("http://ana-d.ikhon.com.br/proton/ProtocoloPublico/Home/Index"); //url de redirecinamento após autenticação
            string link = $"{urlAuthorize}authorize?" +
                $"response_type=code&" +
                $"client_id={_clientId}&" +
                $"scope={scope}&" +
                $"redirect_uri={redirectUrl}&" +
                $"nonce={nonce}&" +
                $"state={state}";
            TempData["LinkGovBr"] = link;

            return View();
        }

        public ActionResult Autenticate(dynamic json)
        {
            return View();
        }
    }
}