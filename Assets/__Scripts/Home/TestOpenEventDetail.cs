using Owlet.UI;
using Owlet.UI.Popups;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class TestOpenEventDetail : MonoBehaviour
    {

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OpenEventDetail);
        }


        async void OpenEventDetail()
        {
            TestEventDetailPopup popup = await PopupManager.instance.OpenUI<TestEventDetailPopup>(Keys.Popup.EventDetail, 0);
            popup.SetupUI("AAAAAA");
        }
    }
}
