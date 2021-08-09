using System;
using System.Collections.Generic;

namespace TirntirobSharp
{
    class ListStrStr
    {
        class Item
        {
            public string Key;
            public string Value;
            public Item(string key, string value = null)
            {
                Key = key;
                Value = value;
            }
        }
        
        private readonly List<Item> list = new List<Item>();

        private Item GetItem(string key)
        {
            foreach (var i in list) if (i.Key == key) return i;
            return null;
        }

        private Item GetItemByValue(string value)
        {
            foreach (var i in list) if (i.Value == value) return i;
            return null;
        }

        public bool Contains(string value) => GetItemByValue(value) != null;

        public bool ContainsKey(string key) => GetItem(key) != null;

        public void Set(string key, string value)
        {
            var i = GetItem(key);
            if (i==null) list.Add(new Item(key,value));
            else i.Value = value;
        }

        public String Get(string key) => GetItem(key)?.Value;

        public String GetKey(string value) => GetItemByValue(value)?.Key;

        public List<string> Keys()
        {
            var l = new List<string>();
            foreach (var i in list) l.Add(i.Key);
            return l;
        }

        public List<string> Values()
        {
            var l = new List<string>();
            foreach (var i in list) l.Add(i.Value);
            return l;
        }
    }
}
