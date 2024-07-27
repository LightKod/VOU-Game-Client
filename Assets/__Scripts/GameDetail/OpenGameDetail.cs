using Owlet.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class OpenGameDetail : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnOpenGameDetail);
        }

        async void OnOpenGameDetail()
        {
            GameDetailPopup popup = await PopupManager.instance.OpenUI<GameDetailPopup>(Keys.Popup.GameDetail, 0);
        }
    }
}
