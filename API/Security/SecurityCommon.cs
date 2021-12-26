using System;
using System.Text;

namespace API.Security.Jwt
{
    public class SecurityCommon
    {
        public const string JwtAuthenticationScheme = "Bearer";
        /// <summary>
        /// Create hash password md5
        /// </summary>
        /// <param name="plainText">Plain text input</param>
        /// <returns></returns>
        public static string CreateHash(string plainText)
        {
            var x = System.Security.Cryptography.MD5.Create();
            var data = Encoding.ASCII.GetBytes(plainText);
            data = x.ComputeHash(data);
            return Convert.ToBase64String(data);
        }
    }
}