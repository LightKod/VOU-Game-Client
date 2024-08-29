using Newtonsoft.Json;
using Owlet.Systems.SceneTransistions;
using PlasticPipe.PlasticProtocol.Messages;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VOU
{
    public class HomeSceneManager : MonoBehaviour
    {
        List<EventModel> featureEvents;
        [SerializeField] EventScroller featureEventScroller;

        private void Start()
        {
            FetchFeatureEvents();
        }

        async void FetchFeatureEvents()
        {
            await HttpClient.GetRequest(ServiceHelper.GetURL(Env.Routes.Event.All), async (string res) =>
            {
                featureEvents = JsonConvert.DeserializeObject<List<EventModel>>(res);

                List<string> imgUrls = featureEvents.Select(x => x.poster).ToList();

                await ImageCache.LoadImages(imgUrls, 5);
                Debug.Log("Done loading");

                featureEventScroller.SetData(featureEvents);
            }
            , (msg) =>
            {
                Debug.Log($"Error fetching feature events: {msg}");
            });


            SceneTransistion.instance.DisableLoadingScreen();
        }
    }
}
