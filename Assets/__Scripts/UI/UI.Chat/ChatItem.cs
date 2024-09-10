using EnhancedUI.EnhancedScroller;
using Owlet;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class ChatItem : EnhancedScrollerCellView
    {
        [SerializeField] Image userIcon;
        [SerializeField] TextMeshProUGUI txtPlayerName;
        [SerializeField] TextMeshProUGUI txtChatMsg;

        [SerializeField] RefreshRectTransform refresher;

        public async void SetupUI(ChatDataObject chatDataObject)
        {
            txtPlayerName.text = chatDataObject.username;
            txtChatMsg.text = chatDataObject.text;
            if(!chatDataObject.imgUrl.IsNullOrWhitespace() )
            {
                userIcon.sprite = await ImageCache.GetImage(chatDataObject.imgUrl);
            }
            refresher.Refresh();
        }
    }
}
