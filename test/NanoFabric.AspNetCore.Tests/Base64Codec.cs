using System;
using System.Text;

namespace NanoFabric.AspNetCore.Tests
{
    public class Base64Codec
    {
        private readonly Encoding _encoding;

        public Base64Codec(Encoding encoding)
        {
            _encoding = encoding; 
        }

        public Base64Codec()
            : this(Encoding.UTF8)
        { }

        public string Encode(string s)
        {
            var bytes = _encoding.GetBytes(s);
            return Convert.ToBase64String(bytes);
        }

        public byte[] EncodeToBytes(string s)
        {
            return _encoding.GetBytes(Encode(s));
        }

        public string Decode(string s)
        {
            var bytes = Convert.FromBase64String(s);
            return _encoding.GetString(bytes);
        }

        public string DecodeFromBytes(byte[] bytes)
        {
            return Decode(_encoding.GetString(bytes));
        }
    }
}
