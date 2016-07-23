namespace System.Collections.Generic
{
    public static class DictionaryEx
    {
        public static TValue TryGetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }

            return defaultValue;
        }
    }
}
