using Owlet.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class GachaManager : MonoBehaviour
    {
        [SerializeField] Button btnScratch;


        private void Start()
        {
            btnScratch.onClick.AddListener(Scratch);
        }


        async void Scratch()
        {
            GachaItemModel item = new GachaItemModel
            {
                image = "https://picsum.photos/200",
                name = "Test Item",
                description = "Default description"
            };

            ScratchCardPopup scratchCardPopup = await PopupManager.instance.OpenUI<ScratchCardPopup>(Keys.Popup.ScratchCard, 1, false);
            scratchCardPopup.SetupUI(item);
            scratchCardPopup.EnableUI();
        }
    }
}
