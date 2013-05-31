using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                throw new KeyNotFoundException("There is no fixture by that name: " + binder.Name);
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
        #endregion
    }
}
