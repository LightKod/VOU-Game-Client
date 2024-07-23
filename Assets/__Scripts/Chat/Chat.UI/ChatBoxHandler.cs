using Lean.Pool;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
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

        void UpdateChatHistory(string playerName, string chatMsg)
        {
            ChatItem item = LeanPool.Spawn(chatItemPrefab, chatHistoryHolder);
            item.SetupUI(playerName, chatMsg);
        }
    }
}
