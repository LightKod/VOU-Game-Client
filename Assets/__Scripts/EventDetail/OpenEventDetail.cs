using Owlet.UI;
using Owlet.UI.Popups;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class OpenEventDetail : MonoBehaviour
    {

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnOpenEventDetail);
        }


        async void OnOpenEventDetail()
        {
            EventDetailPopup popup = await PopupManager.instance.OpenUI<EventDetailPopup>(Keys.Popup.EventDetail, 0);
        }
    }
}
