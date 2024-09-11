using Cysharp.Threading.Tasks;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using Owlet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace VOU
{

    public class FirebaseRemoteConfigManager : Singleton<FirebaseRemoteConfigManager>
    {
        private const string HOST = nameof(HOST);
     
    
        public Task FetchRemoteConfig()
        {
            FirebaseRemoteConfig.DefaultInstance.SetConfigSettingsAsync(new ConfigSettings
            {
                MinimumFetchIntervalInMilliseconds = 1000,
            });

            Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync();
            return fetchTask.ContinueWithOnMainThread(FetchComplete);
        }

        private void FetchComplete(Task fetchTask)
        {
            if (!fetchTask.IsCompleted)
            {
                Debug.LogError("Retrieval hasn't finished.");
                return;
            }

            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            var info = remoteConfig.Info;
            if (info.LastFetchStatus != LastFetchStatus.Success)
            {
                Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
                return;
            }

            // Fetch successful. Parameter values must be activated to use.
            remoteConfig.ActivateAsync()
              .ContinueWithOnMainThread(
                task => {
                    Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");

                    Debug.Log("Total values: "+remoteConfig.AllValues.Count);

                    foreach (var item in remoteConfig.AllValues)
                    {
                        print("Key :" + item.Key);
                        print("Value: " + item.Value.StringValue);
                    }
                });
        }
    }
}
