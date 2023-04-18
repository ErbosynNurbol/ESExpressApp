using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ESExpressApp.Models;

namespace ESExpressApp.Helpers
{
   public class APIHelper
    {
        public const string baseAddress = "https://www.esexpress.kz";
        //public static string baseAddress = DeviceInfo.Current.Platform == DevicePlatform.iOS ? "http://127.0.0.1:5091": "http://10.0.2.2:5091";
        private static readonly HttpClient _httpClient = new HttpClient();
        public static Models.AjaxMsgModel Query(string path,Dictionary<string, string> dicParameters)
        {
            return Task.Run(async () =>
            {
                AjaxMsgModel ajaxMsg = null;
                try
                {
                    if (_httpClient.BaseAddress == null)
                    {
                        _httpClient.Timeout = TimeSpan.FromSeconds(30);
                        _httpClient.BaseAddress = new Uri(baseAddress);
                        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    }
                    var dicData = new Dictionary<string, string>
                    {
                      {"language", AppSingleton.GetInstance().GetCurrentLanguage()?.Culture??string.Empty},
                      {"qarToken", AppSingleton.GetInstance().GetLoginInfo()?.QarToken??string.Empty}
                    };

                    if (dicParameters != null && dicParameters.Count() > 0)
                    {
                        dicParameters.ToList().ForEach(x => dicData.Add(x.Key, x.Value));
                    }
                    var content = new FormUrlEncodedContent(dicData);
                    var result = await _httpClient.PostAsync(path, content);
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string returnValue = result.Content.ReadAsStringAsync().Result;
                        ajaxMsg = JsonHelper.DeserializeObject<AjaxMsgModel>(returnValue);
                    }
                }
                catch(Exception ex)
                {
                    ajaxMsg = null;
                }
                return ajaxMsg;
            }).Result;
        }
        static void Write(Stream s, Byte[] bytes)
        {
            using (var writer = new BinaryWriter(s))
            {
                writer.Write(bytes);
            }
        }
        public static AjaxMsgModel UploadAvatar(Stream streamData)
        {
            return Task.Run(async () =>
            {
                AjaxMsgModel ajaxMsg = null;
                try
                {
                    if (_httpClient.BaseAddress == null)
                    {
                        _httpClient.Timeout = TimeSpan.FromSeconds(30);
                        _httpClient.BaseAddress = new Uri(baseAddress);
                        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    }
                    var dicData = new Dictionary<string, string>
                    {
                      {"language", AppSingleton.GetInstance().GetCurrentLanguage()?.Culture},
                      {"qarToken", AppSingleton.GetInstance().GetLoginInfo()?.QarToken}
                    };
                    var content = new MultipartFormDataContent();
                    content.Add(new StringContent(AppSingleton.GetInstance().GetCurrentLanguage()?.Culture), "language");
                    content.Add(new StringContent(AppSingleton.GetInstance().GetLoginInfo()?.QarToken), "qarToken");
                    var streamContent = new StreamContent(streamData);
                    streamContent.Headers.Add("Content-Type", "image/jpeg");
                    streamContent.Headers.Add("Content-Length", streamData.Length.ToString());
                    content.Add(streamContent, "avatarFile", "avatar.jpg");
                    var result = await _httpClient.PostAsync("/API/User/UploadAvatar", content);
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string returnValue = result.Content.ReadAsStringAsync().Result;
                        ajaxMsg = JsonHelper.DeserializeObject<AjaxMsgModel>(returnValue);
                    }
                }
                catch(Exception ex)
                {
                    ajaxMsg = null;
                }
                return ajaxMsg;
            }).Result;
        }
    }
}
