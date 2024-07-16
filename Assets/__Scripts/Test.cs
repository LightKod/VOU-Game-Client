using Sirenix.OdinInspector;
using SocketIOClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

namespace VOU
{
    public class Test : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        [Button]
        async void Socket()
        {
            var uri = new Uri("http://localhost:3000");
            var socket = new SocketIOUnity(uri, new SocketIOOptions
            {
                ExtraHeaders = new Dictionary<string, string>
                {
                    {"Authorization", "bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6MSwiaWF0IjoxNzIwNzg2OTgzLCJleHAiOjE3MjA3OTA1ODN9.lpDU7O-CO14EZWjiAZU_p89j72YY6luAUcu870M8O9Y" }
                }
                ,
                Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
            });

            socket.OnConnected += (sender, e) =>
            {
                Debug.Log("Connected");
            };

            await socket.ConnectAsync();
        }
    }
}
