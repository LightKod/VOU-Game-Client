using Firebase;
using Firebase.Extensions;
using Owlet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class FirebaseManager : Singleton<FirebaseManager>
    {
        public static bool isInitialized = false;


        private void Start()
        {
            InitializeFirebase();
        }

        private void InitializeFirebase()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    Debug.Log(">>> Firebase is ready");
                    FirebaseRemoteConfigManager.instance.FetchRemoteConfig();
                    isInitialized = true;
                }
                else
                {
                    Debug.LogError($">>> Could not resolve all Firebase dependencies: {dependencyStatus}");
                }
            });
        }
    }
}
