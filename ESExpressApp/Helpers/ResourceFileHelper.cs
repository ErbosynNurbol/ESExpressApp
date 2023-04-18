using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ESExpressApp.Helpers
{
    internal class ResourceFileHelper
    {
        public static string GetHtmlFileContent(string fileName)
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(HtmlResourceExtension)).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(fileName);
            string html = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            return html;
        }

        public static string GetHtmlByVideoEmbedCode(string embedCode)
        {
            if (embedCode.ToLower().Contains("iframe"))
            {
                return ResourceFileHelper.GetHtmlFileContent("Asiaopt.Resources.Templates.VideoPlayerTemplate.html")
                .Replace("{HtmlContent}", $"<div class=\"plyr__video-embed\" id=\"iframe-player\">{embedCode}</div>")
                .Replace("{baseUrl}", APIHelper.baseAddress);
            }
            else
            {
                return ResourceFileHelper.GetHtmlFileContent("ESExpressApp.Resources.Templates.VideoPlayerTemplate.html")
                .Replace("{HtmlContent}", embedCode)
               .Replace("{baseUrl}", APIHelper.baseAddress);
            }
        }
    }
}
