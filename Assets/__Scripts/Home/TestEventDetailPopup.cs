using Owlet.UI.Popups;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class TestEventDetailPopup : Popup
    {
        public void SetupUI(string data)
        {
            Debug.Log(data);
        }
    }
}
