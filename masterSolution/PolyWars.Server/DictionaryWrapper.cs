using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.Server {
    public interface IDictionaryADT<TKey, TValue> {
        
    }
    class DictionaryADT<TKey, TValue>  {
        IDictionary<TKey, TValue> dictionary;

        public DictionaryADT(Dictionary<TKey, TValue> d) {
            dictionary = d;
        }
        public bool TryRemove(TKey key, out TValue value) {
            if(dictionary.ContainsKey(key)) {
                TValue v = dictionary[key];
                value = v;
                return true;
            } else {
                value = default(TValue);
                return false;
            }
        }
        public bool TryAdd(TKey key, TValue value) {
            if(dictionary.ContainsKey(key)) {
                return false;
            } else {
                dictionary.Add(key, value);
                return true;
            }
        }
    }
}
