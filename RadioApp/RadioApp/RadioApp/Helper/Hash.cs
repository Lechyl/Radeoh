using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
namespace RadioApp.Helper
{
    public class Hash
    {

      /*  public static string CreateSalt(int size)
        {

            //Secure random generate salt
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            byte[] buff = new byte[size];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }*/

        public static string CreateHash(string mypassword)
        {
            //append password and salt
            //convert password and salt to byte
            /*  byte[] bytePassword = System.Text.Encoding.UTF8.GetBytes(password + salt);

              HashAlgorithm hash = new SHA256CryptoServiceProvider();

              //hashed password
              byte[] hashPassword = hash.ComputeHash(bytePassword);*/

            var myHash = BCrypt.Net.BCrypt.EnhancedHashPassword(mypassword, BCrypt.Net.HashType.SHA384,16);
            return myHash;
        }
        public static bool VerifyHash(string mypassword, string hashedPassword)
        {
            //hasing source to compare hashed source with hashed password from Database.
            var validatePassword = BCrypt.Net.BCrypt.EnhancedVerify(mypassword, hashedPassword, BCrypt.Net.HashType.SHA384);

            return validatePassword;

        }
    }
}
