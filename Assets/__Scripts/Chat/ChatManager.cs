using Owlet;
using SocketIOClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class ChatManager : Singleton<ChatManager>
    {
        ChatService chatService;

        bool isConnected = false;
        string roomID = "20";

        public static Action<string, string> onChatReceived;

        private void Start()
        {
            SetupConnection(roomID);
        }

        private void OnDestroy()
        {
            chatService?.Dispose();
        }

        public async void SetupConnection(string roomID)
        {
            chatService = new();
            await chatService.CreateConnection();
            chatService.JoinRoom(roomID);
            chatService.On(ChatService.EVENT_RECEIVED_CHAT ,OnChatReceived);
            isConnected = true;
        }


        public void SendChatMessage(string message)
        {
            if (!isConnected) return;
            chatService?.SendChat(roomID, message);
        }

        void OnChatReceived(SocketIOResponse res) 
        {
            string name = res.GetValue<string>(0);
            string msg = res.GetValue<string>(1);
            onChatReceived?.Invoke(name, msg);
        }
    }
}
