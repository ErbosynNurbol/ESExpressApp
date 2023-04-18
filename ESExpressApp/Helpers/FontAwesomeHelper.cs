using FontAwesome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ESExpressApp.Helpers
{
   public class FontAwesomeHelper
    {
        //Look AssemblyInfo.cs file
        public static string GetFontFamilyNameByClass(string className)
        {
            string[] cArr = className.Split(null);
            string fontFamily = "fregular";
            if (cArr.Length >= 1)
            {
                switch (cArr[0].Trim().ToLower())
                {
                    case "fas": { fontFamily = "fsolid"; } break;
                    case "far": { fontFamily = "fregular"; } break;
                    case "fal": { fontFamily = "flight"; } break;
                    case "fad": { fontFamily = "fduotone"; } break;
                    case "fab": { fontFamily = "fbrands"; } break;
                }
            }
            return fontFamily;
        }
        public static object GetFontUnicodeValueByClass(string className)
        {
            string[] cArr = className.Split(null);
            string propertyName = "";
            if (cArr.Length >=2)
            {
                string cName = cArr[1].Length > 3 ? cArr[1].Substring(3) : cArr[1];
                var split = cName.Split('-');
                foreach (var s in split)
                {
                    propertyName += FirstCharToUpper(s);
                }
            }
            if (!string.IsNullOrEmpty(propertyName))
            {
                Type type = typeof(FontAwesomeIcons);
                var field = type?.GetField(propertyName);
                var value = field?.GetValue(null);
                return field?.GetValue(null);
            }
            return null;
        }

        private static string FirstCharToUpper(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            if (input.Length == 1)
            {
                return input[0].ToString().ToUpper();
            }

            return input[0].ToString().ToUpper() + input.Substring(1);
        }


    }
}
