using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using ayo.Interfaces;

namespace ayo.Hashes
{
    public class WpaHashFunction : IHashFunction
    {
        private string _pass;
        private byte[] _ssidBytes;

        public string DoHash(string ssidName, string pass)
        {
            _ssidBytes = Encoding.ASCII.GetBytes(ssidName);
            _pass = pass;
            Rfc2898DeriveBytes pbkdf2;

            //little magic here
            //Rfc2898DeriveBytes class has restriction of salt size to >= 8
            //but rfc2898 not (see http://www.ietf.org/rfc/rfc2898.txt)
            //we use Reflection to setup private field to avoid this restriction

            if (_ssidBytes.Length >= 8)
                pbkdf2 = new Rfc2898DeriveBytes(_pass, _ssidBytes, 4096);
            else
            {
                //use dummy salt here, we replace it later vie reflection
                pbkdf2 = new Rfc2898DeriveBytes(_pass, new byte[] {0, 0, 0, 0, 0, 0, 0, 0}, 4096);

                var saltField = typeof (Rfc2898DeriveBytes).GetField("m_salt",
                    BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
                saltField.SetValue(pbkdf2, _ssidBytes);
            }

            //get 256 bit PMK key
            return BitConverter.ToString(pbkdf2.GetBytes(32)).Replace("-", "");
        }
    }
}