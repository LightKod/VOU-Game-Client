using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Owlet.UI.Popups;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class EventDetailPopup : Popup
    {
        [SerializeField] Image imgPoster; 
        [SerializeField] TextMeshProUGUI txtEventName; 
        [SerializeField] TextMeshProUGUI txtEventDetail;

        int eventId;

        public void SetEventId(int eventId)
        {
            this.eventId = eventId;
        }


        protected override async UniTask FetchData()
        {
            await FetchEventData();
        }


        async UniTask FetchEventData()
        {
            await HttpClient.GetRequest(HttpClient.GetURL($"{Env.Routes.Event.GetWithID}/{eventId}"), (res) =>
            {
                EventModel eventModel = JsonConvert.DeserializeObject<EventModel>(res);
                UpdateUI(eventModel);
            },
            (msg) =>
            {
                Debug.Log($"Fetch event data failed: {msg}");
            });
        }

        async void UpdateUI(EventModel eventModel)
        {
            txtEventName.text = eventModel.name;
            txtEventDetail.text = eventModel.description;

            imgPoster.sprite = await ImageCache.GetImage(eventModel.poster);
        }
    }
}
