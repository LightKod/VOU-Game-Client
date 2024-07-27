using Owlet.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class OpenInputAccountDetail : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnOpenInputUserDetail);
        }

        async void OnOpenInputUserDetail()
        {
            InputAccountDetailPopup popup = await PopupManager.instance.OpenUI<InputAccountDetailPopup>(Keys.Popup.InputAccountDetail, 2);
        }
    }
}
