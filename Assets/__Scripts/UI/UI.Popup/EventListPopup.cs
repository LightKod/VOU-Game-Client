using Cysharp.Threading.Tasks;
using EnhancedUI.EnhancedScroller;
using Newtonsoft.Json;
using Owlet.UI.Popups;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using System.Linq;
using System;

namespace VOU
{
    public class EventListPopup : Popup, IEnhancedScrollerDelegate
    {
        [SerializeField] EnhancedScrollerCellView cellViewPrefab;
        [SerializeField] EnhancedScroller scroller;
        List<EventModel> _data = new();

        [SerializeField] SearchBarManager searchBar;

        private bool isLoadingNew;

        int currentPage = 1;
        string searchQuery;

        int totalPages;
        int totalItems;

        private void Awake()
        {
            scroller.Delegate = this;
            searchBar.onSearchValueChanged += SearchEvent;
            scroller.scrollerScrolled = ScrollerScrolled;
        }

        protected override async void OnEnableUI()
        {
            searchQuery = "";
            await FetchEvent(1, searchQuery, SetData);
            base.OnEnableUI();
        }

        async void SearchEvent(string query)
        {
            searchQuery = query;
            if (isLoadingNew) return;
            isLoadingNew = true;
            await FetchEvent(1, query, SetData);
        }


        async void LoadMoreData()
        {
            if (isLoadingNew) return;
            if (currentPage >= totalPages)
            {
                return;
            }

            isLoadingNew = true;
            currentPage += 1;
            await FetchEvent(currentPage, searchQuery, AppendData);

        }

        void SetData(List<EventModel> data)
        {
            _data = data;
            scroller.ReloadData();
            scroller.ScrollPosition = 0;
            isLoadingNew = false;
            currentPage = 1;
        }

        void AppendData(List<EventModel> data)
        {
            float prevPos = scroller.ScrollPosition;
            _data.AddRange(data);
            scroller.ReloadData();

            scroller.ScrollPosition = prevPos;

            isLoadingNew = false;
        }

        private void ScrollerScrolled(EnhancedScroller scroller, Vector2 val, float scrollPosition)
        {
            if (scroller.NormalizedScrollPosition >= 0.999f && !isLoadingNew)
            {
                LoadMoreData();
            }
        }

        async UniTask FetchEvent(int page, string search, Action<List<EventModel>> onFetched)
        {
            NameValueCollection query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query.Add("page", page.ToString());
            query.Add("search", search);

            string queryString = query.ToString();
            Debug.Log(queryString);
            string url = ServiceHelper.GetURL(Env.Routes.Event.Search) + $"?{queryString}";
            Debug.Log(url);
            await HttpClient.GetRequest(url, true,
                async (string res) =>
                {
                    Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    string eventListStr = data["data"].ToString();
                    string maxPageStr = data["totalPages"].ToString();

                    List<EventModel> result = JsonConvert.DeserializeObject<List<EventModel>>(eventListStr);
                    totalPages = int.Parse(maxPageStr);
                    //totalItems = (data["totalItems"]); 

                    List<string> imgUrls = result.Select(x => x.poster).ToList();
                    int lazyLoadAmount = Mathf.Min(8, result.Count);
                    await ImageCache.LoadImages(imgUrls, lazyLoadAmount);
                    onFetched?.Invoke(result);
                }
               , (msg) =>
               {
                   Debug.Log($"Error fetching events: {msg}");
               });
        }


        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            EventModel model = _data[dataIndex];
            EventItemCellView cellView = scroller.GetCellView(cellViewPrefab) as EventItemCellView;
            cellView.SetData(model);
            return cellView;
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return 186;
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return _data.Count;
        }
    }
}
