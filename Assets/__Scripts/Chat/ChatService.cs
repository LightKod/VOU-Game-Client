using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using SocketIOClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public static class ChatService
    {

        const string AUTH_KEY = "bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6MiwiaWF0IjoxNzIxNzAyODM1LCJleHAiOjM2MDAxNzIxNzAyODM1fQ.Vv3__g5tCx8wV3_OTnI4TRQYrNkNhKsclbYgK-eOHFY";
        const string HOST = "http://localhost:3000/chat";

        public static Action<string, string> onChatReceived;

        [Button("Connect")]
        public static async UniTask<SocketIOUnity> CreateConnection()
        {
            var uri = new Uri(HOST);
            var socket = new SocketIOUnity(uri, new SocketIOOptions
            {
                ExtraHeaders = new Dictionary<string, string>
                {
                    {"Authorization", AUTH_KEY}
                }
                ,
            });

            socket.OnConnected += (sender, e) =>
            {
                Debug.Log("Chat Socket Connected");
            };

            socket.OnDisconnected += (sender, e) =>
            {
                Debug.Log("Chat Socket Disconnected");
            };
            socket.OnUnityThread("message", (SocketIOResponse res) =>
            {
                Debug.Log(res.ToString());
            });
            socket.OnUnityThread("chat-received", (SocketIOResponse res) =>
            {
                Debug.Log(res.ToString());
                string name = res.GetValue<string>(0);
                string msg = res.GetValue<string>(1);
                onChatReceived?.Invoke(name, msg);
            });

            await socket.ConnectAsync();
            return socket;
        }

        public static void JoinRoom(SocketIOUnity socket, string roomID)
        {
            socket.Emit("joinRoom", roomID);
        }

        public static void SendChat(SocketIOUnity socket, string roomID, string message)
        {
            socket.Emit("message", roomID, message);
        }
    }
}
