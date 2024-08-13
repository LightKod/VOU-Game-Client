using Cysharp.Threading.Tasks;
using Lean.Gui;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class InitialLoadingScreen : MonoBehaviour
    {
        [SerializeField] LeanToggle toggle;
        void Start()
        {
            Loading();
        }

        async void Loading()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2f)); //Should be replace by the actual loading
            toggle.TurnOn();
        }
    }
}
