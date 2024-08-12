using Owlet;
using Owlet.UI;
using Owlet.UI.Popups;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    [RequireComponent(typeof(Button))]
    public class ButtonOpenPopup : MonoBehaviour
    {
        [SerializeField] string popupID;
        [SerializeField] int layer = 0;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OpenPopup);
        }

        async void OpenPopup()
        {
            Popup popup = await PopupManager.instance.OpenUI<Popup>(popupID, layer);
            SetData(popup); 
        }

        protected virtual void SetData(Popup popup) 
        { 
        
        }
    }
}
