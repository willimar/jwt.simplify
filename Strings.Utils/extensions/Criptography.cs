using System;
using System.Collections.Generic;
using System.Text;

namespace strings.utils.extensions
{
    public static class Criptography
    {
        public static string GetHash(string[] values)
        {
            var result = string.Empty;

            foreach (var item in values)
            {
                result += item;
            }

            return BitConverter.ToString(new System.Security.Cryptography.SHA256Managed()
                                .ComputeHash(Encoding.UTF8.GetBytes(result))).Replace("-", string.Empty);
        }
    }
}
