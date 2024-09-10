using EnhancedUI.EnhancedScroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class EventScroller : ModelScroller<EventModel>
    {
        public void SetData(List<EventModel> eventModels)
        {
            _data = eventModels;
            scroller.ReloadData();
        }

        public override EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            EventModel model = _data[dataIndex];
            EventItemCellView cellView = scroller.GetCellView(cellViewPrefab) as EventItemCellView;
            cellView.SetData(model);
            return cellView;
        }
    }
}
