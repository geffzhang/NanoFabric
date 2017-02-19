using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NanoFabric.AspNetCore.Tests
{
    public class KeyValues : Core.IHaveKeyValues
    {
        private readonly Base64Codec _codec = new Base64Codec();
        private readonly Dictionary<string, string> _dictionary = new Dictionary<string, string>();

        public KeyValues WithKeyValue(string key, string value)
        {
            KeyValuePutAsync(key, value);

            return this;
        }

        public int Count => _dictionary.Count;

        public Task KeyValuePutAsync(string key, string value)
        {
            _dictionary.Add(key, _codec.Encode(value));

            return Task.FromResult(0);
        }

        public Task<string> KeyValueGetAsync(string key)
        {
            string value;
            if (_dictionary.TryGetValue(key, out value))
            {
                return Task.FromResult(_codec.Decode(value));
            }

            return Task.FromResult(default(string));
        }

        public Task KeyValueDeleteAsync(string key)
        {
            if (_dictionary.ContainsKey(key))
            {
                _dictionary.Remove(key);
            }

            return Task.FromResult(0);
        }

        public Task KeyValueDeleteTreeAsync(string prefix)
        {
            var deletes = _dictionary.Where(x => x.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)).ToArray();
            for (int i = deletes.Length - 1; i >= 0; i--)
            {
                _dictionary.Remove(deletes[i].Key);
            }

            return Task.FromResult(0);
        }

        public Task<string[]> KeyValuesGetKeysAsync(string prefix)
        {
            var result = _dictionary
                .Where(kvp => string.IsNullOrEmpty(prefix) || kvp.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .Select(kvp => kvp.Key)
                .ToArray();

            return Task.FromResult(result);
        }

        public static implicit operator List<KeyValuePair<string, string>>(KeyValues keyValues)
        {
            return keyValues._dictionary.ToList();
        }
    }
}
