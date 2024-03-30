namespace demoAPI.Common.Helper
{
    public static class DictionaryUtils
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            TValue value;
            return (key != null && dictionary.TryGetValue(key, out value)) ? value : defaultValue;
        }

        public static IDictionary<TKey, TValue> Remove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> key)
        {
            foreach (var k in key)
            {
                dictionary.Remove(k);
            }
            return dictionary;
        }

        public static IDictionary<TKey, TValue> AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }

            return dictionary;
        }

        public static void AddOrUpdate<TKey, TValue>(Dictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] = value;
            }
            else
            {
                dic.Add(key, value);
            }
        }

        public static void AddOrUpdateList<TKey, TValue>(Dictionary<TKey, List<TValue>> dic, TKey key, TValue value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key].Add(value);
            }
            else
            {
                dic.Add(key, new List<TValue>() { value });
            }
        }

        public static void AddRange<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection)
        {
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    if (!source.ContainsKey(item.Key)) { source.Add(item.Key, item.Value); }
                }
            }
        }
    }
}
