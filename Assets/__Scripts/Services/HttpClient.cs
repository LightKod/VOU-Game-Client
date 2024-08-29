using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        public static async UniTask GetRequest(string url, System.Action<string> onSuccess = null, System.Action<string> onError = null)
        {
            using UnityWebRequest www = UnityWebRequest.Get(url);
            try
            {
                await www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    onSuccess?.Invoke(www.downloadHandler.text);
                }
                else
                {
                    onError?.Invoke(www.error);
                }
            }
            catch (Exception e)
            {
                onError?.Invoke(e.Message);
            }
        }

        public static async UniTask<Sprite> GetSpriteFromURL(string url)
        {
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
            {
                var asyncOp = www.SendWebRequest();

                while (asyncOp.isDone == false)
                    await Task.Delay(1000 / 30);

                if( www.result!=UnityWebRequest.Result.Success )
                {
#if DEBUG
                    Debug.Log($"{www.error}, URL:{www.url}");
#endif

                    return null;
                }
                else
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(www);
                    return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                }
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
