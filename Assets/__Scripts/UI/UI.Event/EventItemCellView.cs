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
        [SerializeField] Image imgBrandIcon;

        EventModel eventModel;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OpenEventDetail);
        }

        async void OpenEventDetail()
        {
            if (eventModel == null) return;
            var eventDetailPopup = await PopupManager.instance.OpenUI<EventDetailPopup>(Keys.Popup.EventDetail,0);
            eventDetailPopup.SetEventId(eventModel.id);
        }

        public void SetData(EventModel eventModel)
        {
            this.eventModel = eventModel;
            txtEventName.text = eventModel.name;
        }
    }
}
