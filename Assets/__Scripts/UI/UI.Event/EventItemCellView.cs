using EnhancedUI.EnhancedScroller;
using Owlet.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class EventItemCellView : EnhancedScrollerCellView
    {
        [SerializeField] Image imgPoster;
        [SerializeField] TextMeshProUGUI txtEventName;
        [SerializeField] TextMeshProUGUI txtBrandName;

        EventModel eventModel;


        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OpenEventDetail);
        }

        async void OpenEventDetail()
        {
            if (eventModel == null) return;
            var eventDetailPopup = await PopupManager.instance.OpenUI<EventDetailPopup>(Keys.Popup.EventDetail,0, false);
            eventDetailPopup.SetEventId(eventModel.id);
            eventDetailPopup.EnableUI();
        }

        public async void SetData(EventModel eventModel)
        {
            this.eventModel = eventModel;
            txtEventName.text = eventModel.name;
            imgPoster.sprite = await ImageCache.GetImage(eventModel.poster);
        }
    }
}
