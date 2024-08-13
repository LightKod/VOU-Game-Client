using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace VOU
{
    public static class HttpClient
    {
        public static async UniTask PostRequest(string url, string jsonBody, System.Action<string> onSuccess = null, System.Action<string> onError = null)
        {
            using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
                www.uploadHandler = new UploadHandlerRaw(bodyRaw);
                www.downloadHandler = new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");

                await www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    onError?.Invoke(www.error); // Call onError callback
                }
                else
                {
                    onSuccess?.Invoke(www.downloadHandler.text); // Call onSuccess callback
                }
            }
        }

        public static async UniTask PostRequest(string url, Dictionary<string, string> form, System.Action<string> onSuccess = null, System.Action<string> onError = null)
        {
            using UnityWebRequest www = UnityWebRequest.Post(url, form);
            try
            {
                await www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    onSuccess?.Invoke(www.downloadHandler.text);
                }
            }
            catch(Exception e)
            {
                onError?.Invoke(www.error);
            }
            
        }

        public static string GetURL(string baseUrl, string route)
        {
            if (!baseUrl.EndsWith("/"))
            {
                baseUrl += "/";
            }

            if (route.StartsWith("/"))
            {
                route = route.Substring(1);
            }

            return baseUrl + route;
        }

        public static string GetURL(string route)
        {
            return GetURL(Env.BASE_URL, route);
        }
    }
}
