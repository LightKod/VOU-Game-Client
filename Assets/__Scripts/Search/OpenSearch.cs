using Owlet.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class OpenSearch : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnOpenSearch);
        }

        async void OnOpenSearch()
        {
            SearchPopup popup = await PopupManager.instance.OpenUI<SearchPopup>(Keys.Popup.Search, 0);
        }
    }
}
