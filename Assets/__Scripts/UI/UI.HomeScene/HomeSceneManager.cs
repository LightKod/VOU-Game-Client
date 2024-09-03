using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Owlet.Systems.SceneTransistions;
using PlasticPipe.PlasticProtocol.Messages;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace VOU
{
    public class HomeSceneManager : MonoBehaviour
    {
        List<EventModel> featureEvents;
        List<GameModel> featureGames;
        [SerializeField] EventScroller featureEventScroller;
        [SerializeField] GameScroller featureGameScroller;

        bool isLoadingFeatureEvents = true;
        bool isLoadingPopularGames = true;

        private async void Start()
        {
            FetchFeatureEvents();
            FetchPopularGames();

            await WaitLoading();
            SceneTransistion.instance.DisableLoadingScreen();
        }

        async void FetchFeatureEvents()
        {
            await HttpClient.GetRequest(ServiceHelper.GetURL(Env.Routes.Event.All), async (string res) =>
            {
                featureEvents = JsonConvert.DeserializeObject<List<EventModel>>(res);

                List<string> imgUrls = featureEvents.Select(x => x.poster).ToList();

                await ImageCache.LoadImages(imgUrls, 5);
                Debug.Log("Done loading feature events");

                featureEventScroller.SetData(featureEvents);
                isLoadingFeatureEvents = false;
            }
            , (msg) =>
            {
                Debug.Log($"Error fetching feature events: {msg}");
                isLoadingFeatureEvents = false;
            });
        }

        async void FetchPopularGames()
        {
            await HttpClient.GetRequest(ServiceHelper.GetURL(Env.Routes.Game.All), async (string res) =>
            {
                featureGames = JsonConvert.DeserializeObject<List<GameModel>>(res);

                List<string> imgUrls = featureGames.Select(x => x.poster).ToList();

                await ImageCache.LoadImages(imgUrls, 5);
                Debug.Log("Done loading feature events");

                featureGameScroller.SetData(featureGames);
                isLoadingPopularGames = false;
            }
            , (msg) =>
            {
                Debug.Log($"Error fetching feature events: {msg}");
                isLoadingPopularGames = false;
            });
        }


        async UniTask WaitLoading()
        {
            while (isLoadingFeatureEvents) await Task.Yield();
            while (isLoadingPopularGames) await Task.Yield();
        }
    }
}
