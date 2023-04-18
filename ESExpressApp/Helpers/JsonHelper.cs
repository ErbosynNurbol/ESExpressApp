using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESExpressApp.Helpers
{
    public class JsonHelper
    {

        #region Өбиектіні Json ге өзгерту  +SerializeObject(object obj)
        /// <summary>
        /// Өбиектіні Json ге өзгерту
        /// </summary>
        /// <param name="obj">Өбиекті</param>
        /// <returns>json</returns>
        public static string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        #endregion

        #region Json ды өбиектіге өзгерту +DeserializeObject<T>(string str)
        /// <summary>
        /// Json ды өбиектіге өзгерту
        /// </summary>
        /// <typeparam name="T">Өбиекті түрі</typeparam>
        /// <param name="str">JSON</param>
        /// <returns>Өбиекті</returns>
        public static T DeserializeObject<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
        #endregion
    }
}
