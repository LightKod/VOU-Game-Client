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
        const string EVENT_DEBUG = "debug";
        protected SocketIOUnity socket;
        protected abstract string GetURL();
        public async UniTask<SocketIOUnity> CreateConnection()
        {
            string token = "bearer " + PlayerPrefs.GetString(Keys.PlayerPrefs.User.Token);

            string url = GetURL();
            var uri = new Uri(url);
            var socket = new SocketIOUnity(uri, new SocketIOOptions
            {
                Path = "/socket",
                ExtraHeaders = new Dictionary<string, string>
                {
                    {"Authorization", token}
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
