using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using System;

namespace Owlet
{
    public static class AddressableLoader
    {
        public static HashSet<string> cache = new();

        public static async UniTask<T> Load<T>(string key) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            var type = typeof(T);
            try
            {
                if (type.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    var result = await AddressablesManager.LoadAssetAsync<GameObject>(key);
                    var finalResult = result.Value.GetComponent<T>();

                    if (finalResult == null)
                    {
                        return null;
                    }

                    cache.Add(key);
                    return finalResult;
                }
                else
                {
                    var result = await AddressablesManager.LoadAssetAsync<T>(key);
                    cache.Add(key);
                    return result;
                }
            }
            catch (Exception e)
            {
                    Debug.Log($"Load fail: {key} + {e.Message}");
                return null;
            }
        }
    }
}
