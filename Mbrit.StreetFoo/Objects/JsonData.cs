using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections;

namespace Mbrit.StreetFoo
{
    public class JsonData
    {
        private Dictionary<string, object> Values { get; set; }

        public event EventHandler Changed;

        private const string DataKey = "data-ppl";

        public JsonData()
        {
            this.Values = new Dictionary<string, object>();
        }

        public JsonData(string json)
            : this()
        {
            if (!(string.IsNullOrEmpty(json)))
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                IDictionary loaded = (IDictionary)serializer.DeserializeObject(json);
                foreach (string key in loaded.Keys)
                    this.Values[key] = loaded[key];
            }
        }

        public object GetValueSafe(string key)
        {
            if (this.Values.ContainsKey(key))
                return this.Values[key];
            else
                return null;
        }

        public void SetValue(string key, object value)
        {
            this.Values[key] = value;
            this.OnChanged();
        }

        private void OnChanged()
        {
            if (this.Changed != null)
                this.Changed(this, EventArgs.Empty);
        }

        public object this[string key]
        {
            get
            {
                return this.GetValueSafe(key);
            }
            set
            {
                this.SetValue(key, value);
            }
        }

        public override string ToString()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(this.Values);
            return json;
        }

        public bool ContainsKey(string key)
        {
            return this.Values.ContainsKey(key);
        }

        public List<T> GetValueAsCollectionSafe<T>(string name)
        {
            List<T> results = new List<T>();

            // get...
            object[] values = (object[])this.GetValueSafe(name);
            if (values != null)
            {
                foreach (object value in values)
                    results.Add((T)Convert.ChangeType(value, typeof(T)));
            }

            return results;
        }

        internal void RemoveSafe(string name)
        {
            if (this.Values.ContainsKey(name))
                this.Values.Remove(name);
        }

        public Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> results = new Dictionary<string, object>();
            foreach (string key in this.Values.Keys)
                results[key] = this.Values[key];

            return results;
        }

        public string GetValueAsString(string name)
        {
            object value = this[name];
            return Convert.ToString(value);
        }
    }
}
