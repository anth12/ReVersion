using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ReVersion.Utilities.Helpers
{
    public class AuthenticationHelper
    {
        private static readonly byte[] key =
        {
            77, 102, 11, 92, 95, 142, 90, 114, 209, 74, 175, 42, 172, 39, 7, 72, 128,
            207, 246, 197, 192, 226, 144, 232, 97, 230, 16, 128, 89, 208, 224, 15
        };

        private static readonly byte[] vector =
        {
            250, 111, 103, 143, 216, 172, 112, 191, 212, 233, 46, 148, 144, 18, 80,
            72
        };

        private readonly ICryptoTransform decryptor;
        private readonly UTF8Encoding encoder;
        private readonly ICryptoTransform encryptor;

        public AuthenticationHelper()
        {
            var rm = new RijndaelManaged();
            encryptor = rm.CreateEncryptor(key, vector);
            decryptor = rm.CreateDecryptor(key, vector);
            encoder = new UTF8Encoding();
        }

        public string Encrypt(string unencrypted)
        {
            return Convert.ToBase64String(Encrypt(encoder.GetBytes(unencrypted)));
        }

        public string Decrypt(string encrypted)
        {
            return encoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));
        }

        public byte[] Encrypt(byte[] buffer)
        {
            return Transform(buffer, encryptor);
        }

        public byte[] Decrypt(byte[] buffer)
        {
            return Transform(buffer, decryptor);
        }

        protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            var stream = new MemoryStream();
            using (var cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }
            return stream.ToArray();
        }
    }
}