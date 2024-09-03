using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public static class ImageCache
    {
        public static Dictionary<string, Sprite> cache = new();

        /// <summary>
        /// Return or load an Image if not in cache
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async UniTask<Sprite> GetImage(string url)
        {
            if(cache.ContainsKey(url) && cache[url] != null)
            {
                return cache[url];
            }

            Sprite sprite = await HttpClient.GetSpriteFromURL(url);
            cache.TryAdd(url, sprite);
            return sprite;
        }

        /// <summary>
        /// Load all images into the cache
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="initialAmount">The amount need to be loaded before freeing up current thread</param>
        /// <returns></returns>
        public static async UniTask LoadImages(List<string> urls, int initialAmount)
        {
            initialAmount = Mathf.Clamp(initialAmount, 0, urls.Count - 1);
            List<string> initialLoad = urls.GetRange(0, initialAmount);
            List<string> lazyLoad = urls.GetRange(initialAmount, urls.Count - initialAmount);
            foreach(string url in initialLoad)
            {
                await GetImage(url);
            }

            LoadLazy(lazyLoad);
        }


        /// <summary>
        /// Load without taking up the thread
        /// </summary>
        /// <param name="urls"></param>
        public static async void LoadLazy(List<string> urls)
        {
            foreach (string url in urls)
            {
                await GetImage(url);
            }
        }
    }
}
