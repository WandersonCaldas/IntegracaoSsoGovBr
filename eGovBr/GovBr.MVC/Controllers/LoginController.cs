using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Text;
using System.Net.Http;
using RestSharp;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using GovBr.MVC.Models;

namespace GovBr.MVC.Controllers
{
    public class LoginController : Controller
    {
        private string _clientId = ConfigurationManager.AppSettings["ClientId"];
        private string _clientSecret = ConfigurationManager.AppSettings["ClientSecret"];
        private string _urlBaseAccess = ConfigurationManager.AppSettings["UrlBaseAccess"].ToString();

        public ActionResult Index()
        {
            Random random = new Random();
            string state = random.Next().ToString("X");
            string nonce = random.Next().ToString("X");
            string scope = "openid+(email/phone)+profile+govbr_confiabilidades";//"openid+(email/phone)+profile+govbr_empresa+govbr_confiabilidades";
            string redirectUrl = Url.Encode(""); //url de redirecinamento após autenticação
            string link = $"{_urlBaseAccess}authorize?" +
                $"response_type=code&" +
                $"client_id={_clientId}&" +
                $"scope={scope}&" +
                $"redirect_uri={redirectUrl}&" +
                $"nonce={nonce}&" +
                $"state={state}";
            TempData["LoginGovBr"] = link;

            string linkLogout = $"{_urlBaseAccess}logout?" +
                $"post_logout_redirect_uri={Url.Encode("")}";
            TempData["LogoutGovBr"] = linkLogout;

            return View();
        }

        public ActionResult Autenticate(string code)
        {
            string tokenRequest = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{_clientId}:{_clientSecret}"));
            string resource = $"token?grant_type=authorization_code&code={code}&redirect_uri={Url.Encode("")}";

            var request = new RestRequest(resource,Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", $"Basic {tokenRequest}");

            var client = new RestClient(_urlBaseAccess);
            var response = client.Execute(request);
            var jsonJwt = JsonConvert.DeserializeObject<dynamic>(response.Content);

            //para realizar requisições dentro do gov.br é necessário utilizar o token abaixo.
            //string bearerToken = jsonJwt.access_token.Value;

            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(jsonJwt.id_token.Value);
            var model = new JsonModel(jwtSecurityToken.Payload);

            return View("Index", model);
        }
    }
}