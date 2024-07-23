using Owlet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class ChatManager : Singleton<ChatManager>
    {
        SocketIOUnity socket;

        bool isConnected = false;
        string roomID = "1";
        private void Start()
        {
            SetupConnection(roomID);
        }

        private void OnDestroy()
        {
            DisposeConnection();
        }

        public async void SetupConnection(string roomID)
        {
            socket = await ChatService.CreateConnection();
            ChatService.JoinRoom(socket, roomID);
            isConnected = true;
        }


        public void SendChatMessage(string message)
        {
            if (!isConnected) return;
            ChatService.SendChat(socket, roomID, message);
        }

        void DisposeConnection()
        {
            socket?.Disconnect();
            socket?.Dispose();
        }
    }
}
