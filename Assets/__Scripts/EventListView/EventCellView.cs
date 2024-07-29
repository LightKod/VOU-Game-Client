using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;
using TMPro;
using PlasticPipe.PlasticProtocol.Messages;

namespace VOU
{
    public class EventCellView : EnhancedScrollerCellView
    {
        [SerializeField] private TMP_Text eventTitle;
        [SerializeField] private TMP_Text startDate;
        [SerializeField] private TMP_Text endDate;
        [SerializeField] private TMP_Text brandName;

        public void SetData(EventItem data)
        {
            eventTitle.SetText(data.eventTitle);
            startDate.SetText(data.startDate);
            endDate.SetText(data.endDate);
            brandName.SetText(data.brandName);
        } 
    }
}
