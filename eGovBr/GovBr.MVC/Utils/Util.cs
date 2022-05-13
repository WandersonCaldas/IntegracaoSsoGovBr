using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GovBr.MVC.Utils
{
    public class Util
    {
        public static string MaskCpfCpnj(string numCpfCnpj)
        {
            if (string.IsNullOrEmpty(numCpfCnpj)) return string.Empty;

            // Se for CNPJ
            if (numCpfCnpj.Length == 14)
            {
                return numCpfCnpj.Insert(2, ".").Insert(6, ".").Insert(10, "/").Insert(15, "-");
            }

            // Se for CPF
            else if (numCpfCnpj.Length == 11)
            {
                return numCpfCnpj.Insert(3, ".").Insert(7, ".").Insert(11, "-");
            }
            else
                return numCpfCnpj;

        }

        public static string MaskPhone(string numero)
        {
            //(61)984976060
            if (numero.Length == 11)
                return numero.Insert(0, "(").Insert(3, ")").Insert(9, "-");
            else
                return numero;
        }
    }
}