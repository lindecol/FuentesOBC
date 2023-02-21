using System;
using System.Text.RegularExpressions;

#if NETCF20
namespace MovilidadCF.Validation
{
#else

namespace Desktop.Validation
{
#endif

    /// <summary>
    /// Descripción breve de Validation.
    /// </summary>
    public sealed class Validator
    {
        private static Regex URLExp = new Regex("^(ht|f)tp(s?)\\:\\/\\/[0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*(:(0-9)*)*(\\/?)([a-zA-Z0-9\\-\\.\\?\\,\\'\\/\\\\\\+&amp;%\\$#_]*)?$");
        private static Regex EMailExp = new Regex("^([0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$");

        private Validator()
        {
            // No se permiten instancias de la clase
        }

        public static bool IsURL(string sURL)
        {
            return URLExp.IsMatch(sURL);
        }

        public static bool IsEmail(string sURL)
        {
            return EMailExp.IsMatch(sURL);
        }

        public static bool IsIP(string sIP)
        {
            try
            {
                System.Net.IPAddress.Parse(sIP);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool IsNumber(string sNumber)
        {
            try
            {
                Convert.ToDouble(sNumber);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool IsMatchExpression(string sRegularExpresion, string sValue)
        {
            Regex exp = new Regex(sRegularExpresion);
            return exp.IsMatch(sValue);
        }
    }
}
