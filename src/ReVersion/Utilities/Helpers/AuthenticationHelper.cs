using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ReVersion.Utilities.Helpers
{
    internal class AuthenticationHelper
    {
        private static readonly byte[] Key =
        {
            77, 102, 11, 92, 95, 142, 90, 114, 209, 74, 175, 42, 172, 39, 7, 72, 128,
            207, 246, 197, 192, 226, 144, 232, 97, 230, 16, 128, 89, 208, 224, 15
        };

        private static readonly byte[] Vector =
        {
            250, 111, 103, 143, 216, 172, 112, 191, 212, 233, 46, 148, 144, 18, 80,
            72
        };

        private readonly ICryptoTransform _decryptor;
        private readonly UTF8Encoding _encoder;
        private readonly ICryptoTransform _encryptor;

        public AuthenticationHelper()
        {
            var rm = new RijndaelManaged();
            _encryptor = rm.CreateEncryptor(Key, Vector);
            _decryptor = rm.CreateDecryptor(Key, Vector);
            _encoder = new UTF8Encoding();
        }

        public string Encrypt(string unencrypted)
        {
            return Convert.ToBase64String(Encrypt(_encoder.GetBytes(unencrypted)));
        }

        public string Decrypt(string encrypted)
        {
            return _encoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));
        }

        public byte[] Encrypt(byte[] buffer)
        {
            return Transform(buffer, _encryptor);
        }

        public byte[] Decrypt(byte[] buffer)
        {
            return Transform(buffer, _decryptor);
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