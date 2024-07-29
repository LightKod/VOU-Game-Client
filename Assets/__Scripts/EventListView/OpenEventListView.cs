using Owlet.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class OpenEventListView : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnOpenEventListView);
        }

        async void OnOpenEventListView()
        {
            EventListViewPopup popup = await PopupManager.instance.OpenUI<EventListViewPopup>(Keys.Popup.EventListView, 0);
        }
    }
}
