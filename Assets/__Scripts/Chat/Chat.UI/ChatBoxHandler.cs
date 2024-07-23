using Cysharp.Threading.Tasks;
using Lean.Pool;
using Owlet;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class ChatBoxHandler : MonoBehaviour
    {
        [SerializeField] TMP_InputField inputChat;
        [SerializeField] Button btnSend;

        [SerializeField] ChatItem chatItemPrefab;
        [SerializeField] Transform chatHistoryHolder;
        [SerializeField] RefreshRectTransform refresher;

        private void Awake()
        {
            btnSend.onClick.AddListener(SendChatMessage);
            ChatService.onChatReceived += UpdateChatHistory;
        }

        private void OnDestroy()
        {
            ChatService.onChatReceived -= UpdateChatHistory;
        }

        void SendChatMessage()
        {
            string msg = inputChat.text;
            if (!msg.IsNullOrWhitespace())
            {
                ChatManager.instance.SendChatMessage(msg);
                inputChat.text = "";
            }
        }

        async void UpdateChatHistory(string playerName, string chatMsg)
        {
            ChatItem item = Instantiate(chatItemPrefab, chatHistoryHolder);
            item.SetupUI(playerName, chatMsg);

            await UniTask.DelayFrame(15);

            refresher.Refresh();
        }
    }
}
