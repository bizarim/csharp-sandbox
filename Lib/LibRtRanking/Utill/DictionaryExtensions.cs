using System;
using System.Collections.Generic;

namespace LibRtRanking.Utill
{
    static public class DictionaryExtensions
    {
        static public TValue GetOrCreate<TKey, TValue>(this Dictionary<TKey, TValue> This, TKey Key, Func<TValue> Allocator)
        {
            if (!This.TryGetValue(Key, out TValue Item))
            {
                return This[Key] = Allocator();
            }
            return Item;
        }

        static public TValue GetOrCreate<TKey, TValue>(this Dictionary<TKey, TValue> This, TKey Key) where TValue : new()
        {
            return This.GetOrCreate(Key, () => { return new TValue(); });
        }

        static public TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> This, TKey Key, TValue DefaultValue)
        {
            if (This.TryGetValue(Key, out TValue Item))
            {
                return Item;
            }
            return DefaultValue;
        }
    }
}
