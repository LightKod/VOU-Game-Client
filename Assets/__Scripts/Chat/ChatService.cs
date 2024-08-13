using Cysharp.Threading.Tasks;
using Owlet;
using Sirenix.OdinInspector;
using SocketIOClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace VOU
{
    public class ChatService : SocketService
    {
        const string AUTH_KEY = "bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6MiwiaWF0IjoxNzIxNzAyODM1LCJleHAiOjM2MDAxNzIxNzAyODM1fQ.Vv3__g5tCx8wV3_OTnI4TRQYrNkNhKsclbYgK-eOHFY";

        public const string EVENT_DEBUG = "debug";
        public const string EVENT_SEND_CHAT = "chatSend";
        public const string EVENT_RECEIVED_CHAT = "chatReceived";
        public const string EVENT_JOIN_ROOM = "joinRoom";
        public const string EVENT_LEAVE_ROOM = "leaveRoom";

        protected override string GetURl()
        {
            return HttpClient.GetURL(Env.Routes.Chat.Root);
        }

        public void JoinRoom(string roomID)
        {
            socket.Emit(EVENT_JOIN_ROOM, roomID);
        }

        public void SendChat(string roomID, string message)
        {
            socket.Emit(EVENT_SEND_CHAT, roomID, message);
        }
    }
}
