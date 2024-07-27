using Owlet.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class OpenInputUserDetail : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnOpenInputUserDetail);
        }

        async void OnOpenInputUserDetail()
        {
            InputUserDetailPopup popup = await PopupManager.instance.OpenUI<InputUserDetailPopup>(Keys.Popup.InputUserDetail, 1);
        }
    }
}
