using EnhancedUI.EnhancedScroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public abstract class ModelScroller<T> : MonoBehaviour, IEnhancedScrollerDelegate where T : BaseModel
    {
        [SerializeField] float cellViewSize;

        protected List<T> _data = new();

        public EnhancedScroller scroller;

        public EnhancedScrollerCellView cellViewPrefab;

        public abstract EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex);
        private void Awake()
        {
            scroller.Delegate = this;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return cellViewSize;
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return _data.Count;
        }
    }
}
