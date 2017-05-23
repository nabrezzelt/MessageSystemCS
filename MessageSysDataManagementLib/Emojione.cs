using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MessageSysDataManagementLib
{
    public partial class Emojione
    {
        readonly static string BaseUri = "http://cdn.jsdelivr.net/emojione/assets/png/";

        public static string ReplaceAllShortnamesWithHTML(string input)
        {
            
            foreach (KeyValuePair<string, string> entry in map)
            {
                input = input.Replace(":" + entry.Key + ":", UnicodeToHTML(entry.Value));
            }

            return input;
        }

        public static string ShortnameToUnicode(string shortName)
        {
            string val;
            if (map.TryGetValue(shortName, out val))
            {
                return val;
            }

            return null;
        }

        public static string UnicodeToHTML(string unicode)
        {
            return String.Format("<img class='emojione' src='{0}{1}.png' />", BaseUri, unicode);
        }

        public static string UnicodeToUrl(string unicode)
        {
            return String.Format("{0}{1}.png", BaseUri, unicode);
        }

        public static string ShortnameToUri(string shortName)
        {
            var unicode = ShortnameToUnicode(shortName);
            if (String.IsNullOrEmpty(unicode))
            {
                return null;
            }

            return UnicodeToUrl(unicode);
        }
    }    
}
