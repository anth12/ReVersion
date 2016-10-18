using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ReVersion.Utilities.Helpers
{
    public class EncryptionHelper
    {
        private readonly Random _random;
        private readonly byte[] _key;
        private readonly RijndaelManaged _rm;
        private readonly UTF8Encoding _encoder;

        public EncryptionHelper()
        {
            _random = new Random();
            _rm = new RijndaelManaged();
            _encoder = new UTF8Encoding();
            _key = Convert.FromBase64String("Yjg5NGYzN2YtNDk5MS00Yjg2LWEwNTQtNDJlZWRlZGEzMDJk");
        }
        
        public string Encrypt(string unencrypted)
        {
            var vector = new byte[16];
            _random.NextBytes(vector);
            var cryptogram = vector.Concat(Encrypt(_encoder.GetBytes(unencrypted), vector));
            return Convert.ToBase64String(cryptogram.ToArray());
        }

        public string Decrypt(string encrypted)
        {
            var cryptogram = Convert.FromBase64String(encrypted);
            if (cryptogram.Length < 17)
            {
                throw new ArgumentException("Not a valid encrypted string", nameof(encrypted));
            }

            var vector = cryptogram.Take(16).ToArray();
            var buffer = cryptogram.Skip(16).ToArray();
            return _encoder.GetString(Decrypt(buffer, vector));
        }

        private byte[] Encrypt(byte[] buffer, byte[] vector)
        {
            var encryptor = _rm.CreateEncryptor(_key, vector);
            return Transform(buffer, encryptor);
        }

        private byte[] Decrypt(byte[] buffer, byte[] vector)
        {
            var decryptor = _rm.CreateDecryptor(_key, vector);
            return Transform(buffer, decryptor);
        }

        private byte[] Transform(byte[] buffer, ICryptoTransform transform)
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