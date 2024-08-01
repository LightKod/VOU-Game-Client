using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class VoucherScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
    {
        private SmallList<VoucherItem> _data;

        [SerializeField] private EnhancedScroller scroller;

        [SerializeField] private VoucherCellView cellViewPrefab;
        [SerializeField] private VoucherLoadingCellView loadingCellViewPrefab;

        public int cellHeight;
        public int pageCount;

        private bool _loadingNew;

        private void Start()
        {
            scroller.Delegate = this;
            scroller.scrollerScrolled = ScrollerScrolled;
        }

        private void OnEnable()
        {
            _data = new SmallList<VoucherItem>();
            scroller.ReloadData();
            scroller.ScrollPosition = 0;

            LoadData(0);
        }

        private void LoadData(int pageStartIndex)
        {
            // grab the last index of the data to jump to when we are finished
            var previousLastIndex = _data.Count;

            // calculate the last index of the new list
            var lastIndex = _data.Count + pageCount;

            // add data to the list
            for (var i = pageStartIndex; i < lastIndex; i++)
            {
                _data.Add(new VoucherItem() 
                { 
                    voucherName = "Voucher Title " + i.ToString(),
                    voucherCode = i.ToString(),
                    voucherDescription = "Meo meo " + i.ToString(),
                    voucherValue = i.ToString(),
                    expiryDate = "30/07/2024"
                });
            }

            // cache the scroller's position so that we can set it back after the reload
            var scrollPosition = scroller.ScrollPosition;

            // tell the scroller to reload now that we have the data.
            scroller.ReloadData();

            // set the scroller's position back to the cached position
            scroller.ScrollPosition = scrollPosition;

            // toggle off loading new so that we can load again at the bottom of the scroller
            _loadingNew = false;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            if (dataIndex == _data.Count)
            {
                VoucherLoadingCellView loadingCellView = scroller.GetCellView(loadingCellViewPrefab) as VoucherLoadingCellView;

                loadingCellView.name = "Loading...";

                return loadingCellView;
            }
            else
            {
                VoucherCellView cellView = scroller.GetCellView(cellViewPrefab) as VoucherCellView;

                cellView.name = "Cell " + dataIndex.ToString();

                cellView.SetData(_data[dataIndex]);

                // return the cell to the scroller
                return cellView;
            }
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return cellHeight;
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return _data.Count + 1;
        }

        private void ScrollerScrolled(EnhancedScroller scroller, Vector2 val, float scrollPosition)
        {
            // if the scroller is at the end of the list and not already loading
            if (scroller.NormalizedScrollPosition >= 0.999f && !_loadingNew)
            {
                // toggle on loading so that we don't get stuck in a loading loop
                _loadingNew = true;

                // for this example, we fake a delay that would simulate getting new data in a real application.
                // normally you would just call LoadData(_data.Count) directly here, instead of adding the fake
                // 1 second delay.
                StartCoroutine(FakeDelay());
            }
        }

        IEnumerator FakeDelay()
        {
            // wait for one second
            yield return new WaitForSeconds(1f);

            // load the data
            LoadData(_data.Count);
        }
    }
}
