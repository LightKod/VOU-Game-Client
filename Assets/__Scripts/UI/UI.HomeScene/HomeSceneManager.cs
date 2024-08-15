using Newtonsoft.Json;
using PlasticPipe.PlasticProtocol.Messages;
using System.Collections;
using System.Collections.Generic;
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
            await HttpClient.GetRequest(ServiceHelper.GetURL(Env.Routes.Event.All), (string res) =>
            {
                featureEvents = JsonConvert.DeserializeObject<List<EventModel>>(res);
                foreach (var evnt in featureEvents)
                {
                    Debug.Log(evnt.name);
                }
                featureEventScroller.SetData(featureEvents);
                Debug.Log(featureEvents.Count);
            }
            , (msg) =>
            {
                Debug.Log($"Error fetching feature events: {msg}");
            });
        }
    }
}
