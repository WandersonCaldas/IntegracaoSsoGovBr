using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Web;
using GovBr.MVC.Utils;

namespace GovBr.MVC.Models
{
    public class JsonModel
    {
        public JsonModel(JwtPayload payload)
        {
            JsonPayload = JsonConvert.SerializeObject(payload, Formatting.Indented);
            Nome = payload.Claims.Where(c => c.Type == "name").Select(c => c.Value).FirstOrDefault();
            Telefone = Util.MaskPhone(payload.Claims.Where(c => c.Type == "phone_number").Select(c => c.Value).FirstOrDefault());
            Email = payload.Claims.Where(c => c.Type == "email").Select(c => c.Value).FirstOrDefault();
            CPF = Util.MaskCpfCpnj(payload.Claims.Where(c => c.Type == "sub").Select(c => c.Value).FirstOrDefault());
        }

        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string JsonPayload { get; set; }
    }
}