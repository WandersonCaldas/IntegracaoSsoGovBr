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
            //string scope = "openid+(email/phone)+profile+govbr_empresa+govbr_confiabilidades";
            string scope = "openid+(email/phone)+profile+govbr_confiabilidades";
            string redirectUrl = Url.Encode("http://ana-d.ikhon.com.br/proton/ProtocoloPublico/Home/Index"); //url de redirecinamento após autenticação
            string link = $"{_urlBaseAccess}authorize?" +
                $"response_type=code&" +
                $"client_id={_clientId}&" +
                $"scope={scope}&" +
                $"redirect_uri={redirectUrl}&" +
                $"nonce={nonce}&" +
                $"state={state}";
            TempData["LoginGovBr"] = link;

            string linkLogout = $"{_urlBaseAccess}logout?" +
                $"post_logout_redirect_uri={Url.Encode("http://ana-d.ikhon.com.br/proton/ProtocoloPublico/Account/Index")}";
            TempData["LogoutGovBr"] = linkLogout;

            return View();
        }

        public ActionResult Autenticate(string code)
        {
            string tokenRequest = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{_clientId}:{_clientSecret}"));
            
            var client = new RestClient(_urlBaseAccess);
            
            var request = new RestRequest("token",Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", $"Basic {tokenRequest}");

            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", Url.Encode("http://ana-d.ikhon.com.br/proton/ProtocoloPublico/Home/Index"));//Url de redirecionamento

            //request.AddQueryParameter("grant_type", "authorization_code");
            //request.AddQueryParameter("code", code);
            //request.AddQueryParameter("redirect_uri", Url.Encode("http://ana-d.ikhon.com.br/proton/ProtocoloPublico/Account/Index"));//Url de redirecionamento

            var response = client.Execute(request);

            //var jsonJwt = JsonConvert.DeserializeObject<dynamic>(response.Content);
            //var token = jsonJwt.id_token.Value;

            #region Teste manual
            //Foi realizado esse teste manual pois pela requisição não esta retornando,
            //pela aplicação Insomnia foi realizado o request com sucesso e retornado o json abaixo.

            dynamic json = "{"
    +"\"access_token\": \"eyJraWQiOiJyc2ExIiwiYWxnIjoiUlMyNTYifQ.eyJzdWIiOiIzMTM0NTgwODg4NyIsImF1ZCI6ImFuYS1kLmlraG9uLmNvbS5iciIsInNjb3BlIjpbImVtYWlsIiwiZ292YnJfY29uZmlhYmlsaWRhZGVzIiwib3BlbmlkIiwicGhvbmUiLCJwcm9maWxlIl0sImFtciI6WyJwYXNzd2QiXSwiaXNzIjoiaHR0cHM6XC9cL3Nzby5zdGFnaW5nLmFjZXNzby5nb3YuYnJcLyIsInByZWZlcnJlZF91c2VybmFtZSI6IjMxMzQ1ODA4ODg3IiwiZXhwIjoxNjUyMzg5NjM3LCJpYXQiOjE2NTIzODYwMzcsImp0aSI6IjBiZGRjOGI4LTRjYTMtNGE5ZS05ODA1LTM4ZWZhZmU3N2M3OSJ9.hrtRjZqZjTfl7xWwN6FE7kGua7fMoj8FWwUnCG_64zakdDv5bTWRSVUm5Oex2A1OBGm8-SOJXQLEnNgdC67duBwKPJdaLfUIMR3VohHZ2rWMmNNoLaK3u-ObBr92_lK81_6cNszcc9nHQ0HTSLe2_gDUEWUpE6AlI5vmuq6hE48lXymm20BwTr4QDQ14xZPAfgSFRNk--6SzjvQ47kscoZuzGVqVjr4zEqaVS-dKwnBc8fdkVxgypeOVtdEgt8-qzMuFtEfong51iMt0JPua1xudXBHYb24MxjZj869OTIJrRCpWVrVt0_kBQYsnoVFvP-beUHaeYxbAy2Yv31LLew\","
	+"\"token_type\": \"Bearer\","
	+"\"expires_in\": 3599,"
	+"\"scope\": \"phone email openid govbr_confiabilidades profile\","
	+"\"id_token\": \"eyJraWQiOiJyc2ExIiwiYWxnIjoiUlMyNTYifQ.eyJzdWIiOiIzMTM0NTgwODg4NyIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSIsImFtciI6WyJwYXNzd2QiXSwicHJvZmlsZSI6Imh0dHBzOlwvXC9jb250YXMuc3RhZ2luZy5hY2Vzc28uZ292LmJyXC8iLCJraWQiOiJyc2ExIiwiaXNzIjoiaHR0cHM6XC9cL3Nzby5zdGFnaW5nLmFjZXNzby5nb3YuYnJcLyIsInBob25lX251bWJlcl92ZXJpZmllZCI6InRydWUiLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiIzMTM0NTgwODg4NyIsIm5vbmNlIjoiNTVCNUJENkYiLCJwaWN0dXJlIjoiaHR0cHM6XC9cL3Nzby5zdGFnaW5nLmFjZXNzby5nb3YuYnJcL3VzZXJpbmZvXC9waWN0dXJlIiwiYXVkIjoiYW5hLWQuaWtob24uY29tLmJyIiwiYXV0aF90aW1lIjoxNjUyMzg2MDI0LCJzY29wZSI6WyJlbWFpbCIsImdvdmJyX2NvbmZpYWJpbGlkYWRlcyIsIm9wZW5pZCIsInBob25lIiwicHJvZmlsZSJdLCJuYW1lIjoiSm9zw6kgZGEgU2lsdmEgVGVzdGUiLCJwaG9uZV9udW1iZXIiOiI2MTk4NDk3NjA2MCIsImV4cCI6MTY1MjM4NjYzNywiaWF0IjoxNjUyMzg2MDM3LCJqdGkiOiJlYmRkYzNiNy0zZTEzLTRhY2MtYTI0ZC01NGYwZWQ3NTlhZWIiLCJlbWFpbCI6ImxhZ3ZAZW1haWwuY29tIn0.BBCxsVh63_H9A6FAgjCUbdgERo7WmoIqW9JUuVMcfWxyFZiyGEuyhqg7Gvbam6K4X9HN5GYl92QP_8PVNGbkfv5Ld0AHQ-Sz4pqfqBOreC7wBQM7O4s7cturmby7j5agzoFapmGR4EGm2HjCx2dAd3sfOhZzDFrc0SQdvkjzQ-PlwDp7EWIxa9Zj-wtsrj8ADLAVZtGSNOG57wxaEE6nrcUr5DyMicE5ANFTmYTegxfB-jF1b02tyO0yVcKcQZBH58fBZAwRODm5nBSuOoyhA3vk3ToIqfu0WNj-Ds3RDgDvwSixiwqi4MVbiAB0BErwmQMcvGE8BF35JcSkbUHRMA\""
    +"}";
            var jsonC = JsonConvert.DeserializeObject<dynamic>(json);

            var token = jsonC.id_token.Value;
            #endregion

            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);

            var jsonPayload = JsonConvert.SerializeObject(jwtSecurityToken.Payload);

            var model = new JsonModel(jwtSecurityToken.Payload);

            return View("Index", model);
        }
    }
}