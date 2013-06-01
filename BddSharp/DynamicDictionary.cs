using System.Collections.Generic;
using System.Dynamic;

namespace BddSharp
{
    public class DynamicDictionary<T> : DynamicObject
    {
        private Dictionary<string, T> _dictionary = new Dictionary<string, T>();

        #region Dyanmic Magic
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            T data;
            if (!_dictionary.TryGetValue(binder.Name, out data))
            {
                throw new KeyNotFoundException("There is no item with key: " + binder.Name);
            }

            result = data;

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_dictionary.ContainsKey(binder.Name))
            {
                _dictionary[binder.Name] = (T)value;
            }
            else
            {
                _dictionary.Add(binder.Name, (T)value);
            }

            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            T data;

            string key = indexes[0].ToString();
            if (!_dictionary.TryGetValue(key, out data))
            {
                throw new KeyNotFoundException("There is no item with key: " + key);
            }

            result = data;

            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            string key = indexes[0].ToString();

            if (_dictionary.ContainsKey(key))
            {
                _dictionary[key] = (T)value;
            }
            else
            {
                _dictionary.Add(key, (T)value);
            }

            return true;
        }
        #endregion
    }
}
