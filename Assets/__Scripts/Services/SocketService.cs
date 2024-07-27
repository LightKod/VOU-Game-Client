using Cysharp.Threading.Tasks;
using Owlet;
using SocketIOClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace VOU
{
    public abstract class SocketService
    {
        const string AUTH_KEY = "bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6MiwiaWF0IjoxNzIxNzAyODM1LCJleHAiOjM2MDAxNzIxNzAyODM1fQ.Vv3__g5tCx8wV3_OTnI4TRQYrNkNhKsclbYgK-eOHFY";
        const string EVENT_DEBUG = "debug";

        protected abstract string GetURl();

        protected SocketIOUnity socket;

        public async UniTask<SocketIOUnity> CreateConnection()
        {
            string url = GetURl();
            var uri = new Uri(url);
            var socket = new SocketIOUnity(uri, new SocketIOOptions
            {
                ExtraHeaders = new Dictionary<string, string>
                {
                    {"Authorization", AUTH_KEY}
                }
            });

            socket.OnConnected += (sender, e) =>
            {
                Helper.Log($"{GetType()} Connected");
            };

            socket.OnDisconnected += (sender, e) =>
            {
                Helper.Log($"{GetType()} Disconnected");
            };
            socket.OnUnityThread(EVENT_DEBUG, (SocketIOResponse res) =>
            {
                Helper.Log($"{GetType()}: {res}");
            });

            await socket.ConnectAsync();
            this.socket = socket;
            return socket;
        }

        public void Dispose()
        {
            socket?.Disconnect();
            socket?.Dispose();
        }

        public void On(string eventName, Action<SocketIOResponse> callback)
        {
            socket?.OnUnityThread(eventName, callback);
        }
    }
}
