using EnhancedUI.EnhancedScroller;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class EventItemCellView : EnhancedScrollerCellView
    {
        [SerializeField] Image imgPoster;
        [SerializeField] TextMeshProUGUI txtEventName;
        [SerializeField] TextMeshProUGUI txtBrandName;
        [SerializeField] Image imgBrandIcon;

        public void SetData(EventModel eventModel)
        {
            txtEventName.text = eventModel.name;
        }
    }
}
