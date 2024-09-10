using Cysharp.Threading.Tasks;
using EnhancedUI.EnhancedScroller;
using Lean.Pool;
using Owlet;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class ChatBoxHandler : MonoBehaviour, IEnhancedScrollerDelegate
    {
        [Header("View")]
        List<ChatDataObject> _data  = new();
        [SerializeField] float cellSize;
        [SerializeField] EnhancedScroller scroller;
        [SerializeField] EnhancedScrollerCellView cellViewPrefab;
        [SerializeField] EnhancedScrollerCellView spacerCellViewPrefab;

        [Header("Chat Input")]
        [SerializeField] TMP_InputField inputChat;
        [SerializeField] Button btnSend;

        [Header("Others")]
        [SerializeField] RefreshRectTransform refresher;

        [Header("Scroller")]
        /// <summary>
        /// This stores the total size of all the cells,
        /// plus the scroller's top and bottom padding.
        /// This will be used to calculate the spacer required
        /// </summary>
        private float _totalCellSize = 0;

        /// <summary>
        /// Stores the scroller's position before jumping to the new chat cell
        /// </summary>
        private float _oldScrollPosition = 0;
        [SerializeField] int characterWidth = 8;
        [SerializeField] int characterHeight = 35;
        [SerializeField] int nameOffset = 50;


        private void Awake()
        {
            btnSend.onClick.AddListener(SendChatMessage);
            scroller.Delegate = this;
            QuizManager.instance.onChatReceived += UpdateChatHistory;
            _data.Add(new());

        }

        private void OnDestroy()
        {
            QuizManager.instance.onChatReceived -= UpdateChatHistory;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            ChatItem cellView;
            if (dataIndex == 0)
            {
                // this is the first spacer cell
                cellView = scroller.GetCellView(spacerCellViewPrefab) as ChatItem;
                cellView.name = "Spacer";
            }
            else
            {
                cellView = scroller.GetCellView(cellViewPrefab) as ChatItem;
                cellView.SetupUI(_data[dataIndex]);
                cellView.name = _data[dataIndex].username;
            }

            return cellView;
            
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return _data[dataIndex].cellSize;
        }

        public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return _data.Count;
        }

        void SendChatMessage()
        {
            string msg = inputChat.text;
            if (!msg.IsNullOrWhitespace())
            {
                QuizManager.instance.SendChatMessage(msg);
                inputChat.text = "";
            }
        }

        [Button]
        async void UpdateChatHistory(string playerName, string chatMsg)
        {
            AddNewRow(playerName, chatMsg);

            await UniTask.DelayFrame(15);

            refresher.Refresh();
        }


        public void AddNewRow(string playerName, string chatMsg)
        {
            // first, clear out the cells in the scroller so the new text transforms will be reset
            scroller.ClearAll();

            _oldScrollPosition = scroller.ScrollPosition;

            // reset the scroller's position so that it is not outside of the new bounds
            scroller.ScrollPosition = 0;


            // calculate the space needed for the text in the cell

            // get the estimated total width of the text (estimated because the font is assumed to be mono-spaced)
            float totalTextWidth = (float)chatMsg.Length * (float)characterWidth;

            // get the number of rows the text will take up by dividing the total width by the widht of the cell
            //int numRows = Mathf.CeilToInt(totalTextWidth / (scroller.GetComponent<RectTransform>().sizeDelta.x)) + 1;
            int numRows = Mathf.CeilToInt(totalTextWidth / 800f) + 1;

            // get the cell size by multiplying the rows times the character height
            var cellSize = numRows * (float)characterHeight + nameOffset;

            // now we can add the data row
            _data.Add(new()
            {
                username = playerName,
                text = chatMsg,
                imgUrl = "",
                cellSize = cellSize,
            });

            ResizeScroller();

            // jump to the end of the scroller to see the new content.
            // once the jump is completed, reset the spacer's size
            scroller.JumpToDataIndex(_data.Count - 1, 1f, 1f, tweenType: EnhancedScroller.TweenType.easeInOutSine, tweenTime: 0.5f, jumpComplete: ResetSpacer);
        }


        private void ResizeScroller()
        {
            // capture the scroll rect size.
            // this will be used at the end of this method to determine the final scroll position
            var scrollRectSize = scroller.ScrollRectSize;

            // capture the scroller's position so we can smoothly scroll from it to the new cell
            var offset = _oldScrollPosition - scroller.ScrollSize;

            // capture the scroller dimensions so that we can reset them when we are done
            var rectTransform = scroller.GetComponent<RectTransform>();
            var size = rectTransform.sizeDelta;

            // set the dimensions to the largest size possible to acommodate all the cells
            rectTransform.sizeDelta = new Vector2(size.x, float.MaxValue);

            // calculate the total size required by all cells. This will be used when we determine
            // where to end up at after we reload the data on the second pass.
            _totalCellSize = scroller.padding.top + scroller.padding.bottom;
            for (var i = 1; i < _data.Count; i++)
            {
                _totalCellSize += _data[i].cellSize + (i < _data.Count - 1 ? scroller.spacing : 0);
            }

            // set the spacer to the entire scroller size.
            // this is necessary because we need some space to actually do a jump
            _data[0].cellSize = scrollRectSize;

            // reset the scroller size back to what it was originally
            rectTransform.sizeDelta = size;

            // reload the data with the newly set cell view sizes and scroller content size.
            //_calculateLayout = false;
            scroller.ReloadData();

            // set the scroll position to the previous cell (plus the offset of where the scroller currently is) so that we can jump to the new cell.
            scroller.ScrollPosition = (_totalCellSize - _data[_data.Count - 1].cellSize) + offset;
        }

        private void ResetSpacer()
        {
            // reset the spacer's cell size to the scroller's size minus the rest of the cell sizes
            // (or zero if the spacer is no longer needed)
            _data[0].cellSize = Mathf.Max(scroller.ScrollRectSize - _totalCellSize, 0);

            // reload the data to set the new cell size
            scroller.ReloadData(1.0f);
        }
    }
}
