using EnhancedUI.EnhancedScroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class EventScroller : MonoBehaviour, IEnhancedScrollerDelegate
    {
        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            throw new System.NotImplementedException();
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            throw new System.NotImplementedException();
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            throw new System.NotImplementedException();
        }
    }
}
