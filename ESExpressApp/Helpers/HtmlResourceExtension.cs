using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ESExpressApp.Helpers
{
    [ContentProperty(nameof(Source))]
    public class HtmlResourceExtension:IMarkupExtension
    {
        public string Source {  get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null) return null;

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(HtmlResourceExtension)).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(Source);
            string html = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            return html;
        }

    }
}
