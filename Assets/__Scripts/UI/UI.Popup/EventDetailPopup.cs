using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Owlet;
using Owlet.UI.Popups;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class EventDetailPopup : Popup
    {
        [Header("Event")]
        [SerializeField] Image imgPoster; 
        [SerializeField] TextMeshProUGUI txtEventName; 
        [SerializeField] TextMeshProUGUI txtEventDetail;

        [Header("Others")]
        [SerializeField] GameScroller gameScroller;
        [SerializeField] RefreshRectTransform refresher;

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

        async UniTask FetchGameInEventData()
        {
            await HttpClient.GetRequest(HttpClient.GetURL($"{Env.Routes.Game.GetAllInEvent}/{eventId}"), (res) =>
            {
                List<GameModel> gameModels = JsonConvert.DeserializeObject<List<GameModel>>(res);
                gameScroller.SetData(gameModels);
            },
            (msg) =>
            {
                Debug.Log($"Fetch game in event data failed: {msg}");
            });
        }

        async void UpdateUI(EventModel eventModel)
        {
            txtEventName.text = eventModel.name;
            txtEventDetail.text = eventModel.description;

            imgPoster.sprite = await ImageCache.GetImage(eventModel.poster);

            refresher.Refresh();

            await FetchGameInEventData();
        }
    }
}
