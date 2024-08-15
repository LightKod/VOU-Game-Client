using EnhancedUI.EnhancedScroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class EventScroller : MonoBehaviour, IEnhancedScrollerDelegate
    {
        private List<EventModel> _data = new();
        
        public EnhancedScroller scroller;

        public EnhancedScrollerCellView cellViewPrefab;

        private void Awake()
        {
            scroller.Delegate = this;
        }

        public void SetData(List<EventModel> eventModels)
        {
            _data = eventModels;

            scroller.ReloadData();
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            EventItemCellView cellView = scroller.GetCellView(cellViewPrefab) as EventItemCellView;
            cellView.SetData(_data[dataIndex]);
            return cellView;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return 472;
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return _data.Count;
        }
    }
}
