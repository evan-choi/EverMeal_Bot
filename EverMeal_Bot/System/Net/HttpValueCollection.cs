using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace System.Net
{
    public class HttpValueCollection : NameValueCollection
    {
        public HttpValueCollection()
        {
        }

        internal void FillFromString(string s, bool urlencoded, Encoding encoding)
        {
            int l = (s != null) ? s.Length : 0;
            int i = 0;

            while (i < l)
            {
                int si = i;
                int ti = -1;

                while (i < l)
                {
                    char ch = s[i];

                    if (ch == '=')
                    {
                        if (ti < 0)
                            ti = i;
                    }
                    else if (ch == '&')
                    {
                        break;
                    }

                    i++;
                }

                string name = null;
                string value = null;

                if (ti >= 0)
                {
                    name = s.Substring(si, ti - si);
                    value = s.Substring(ti + 1, i - ti - 1);
                }
                else
                {
                    value = s.Substring(si, i - si);
                }

                if (urlencoded)
                {
                    base.Add(
                       HttpUtility.UrlDecode(name, encoding),
                       HttpUtility.UrlDecode(value, encoding));
                }
                else
                {
                    base.Add(name, value);
                }

                if (i == l - 1 && s[i] == '&')
                {
                    base.Add(null, string.Empty);
                }

                i++;
            }
        }

        public override string ToString()
        {
            return ToString(true, null);
        }

        string ToString(bool urlencoded, IDictionary excludeKeys)
        {
            int n = Count;
            if (n == 0)
                return string.Empty;

            StringBuilder s = new StringBuilder();
            string key, keyPrefix, item;

            for (int i = 0; i < n; i++)
            {
                key = GetKey(i);

                if (excludeKeys != null && key != null && excludeKeys[key] != null)
                {
                    continue;
                }
                if (urlencoded)
                {
                    key = HttpUtility.UrlEncode(key);
                }
                keyPrefix = (!string.IsNullOrEmpty(key)) ? (key + "=") : string.Empty;

                ArrayList values = (ArrayList)BaseGet(i);
                int numValues = (values != null) ? values.Count : 0;

                if (s.Length > 0)
                {
                    s.Append('&');
                }

                if (numValues == 1)
                {
                    s.Append(keyPrefix);
                    item = (string)values[0];
                    if (urlencoded)
                        item = HttpUtility.UrlEncode(item);
                    s.Append(item);
                }
                else if (numValues == 0)
                {
                    s.Append(keyPrefix);
                }
                else
                {
                    for (int j = 0; j < numValues; j++)
                    {
                        if (j > 0)
                        {
                            s.Append('&');
                        }
                        s.Append(keyPrefix);
                        item = (string)values[j];
                        if (urlencoded)
                        {
                            item = HttpUtility.UrlEncode(item);
                        }
                        s.Append(item);
                    }
                }
            }

            return s.ToString();
        }
    }
}