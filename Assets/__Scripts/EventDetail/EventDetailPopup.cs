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
        
        public void SetEventId(int eventId)
        {
            FetchEventData(eventId);
        }


        async void FetchEventData(int eventId)
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

        void UpdateUI(EventModel eventModel)
        {
            txtEventName.text = eventModel.name;
            txtEventDetail.text = eventModel.description;
        }
    }
}
