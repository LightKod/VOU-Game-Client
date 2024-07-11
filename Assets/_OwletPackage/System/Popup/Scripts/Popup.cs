using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Gui;

namespace Owlet.UI.Popups
{
    public class Popup : MonoBehaviour
    {
        [SerializeField] protected string ID;

        [SerializeField] protected LeanToggle toggle;

        public Action onOpened;
        public Action onClosed;

        public bool closing { get; private set; }


        public virtual void EnableUI()
        {
            StopAllCoroutines();
            OnEnableUI();
            closing = false;
            toggle.TurnOn();
            onOpened?.Invoke();
        }

        protected virtual void OnEnableUI()
        {

        }

        protected virtual void OnDisableUI()
        {

        }

        public virtual void DisableUI()
        {
            if (closing) return;
            closing = true;
            OnDisableUI();
            toggle.TurnOff();
            onClosed?.Invoke();
            ResetListener();
        }

        void ResetListener()
        {
            onClosed = null;
            onOpened = null;
        }

        public void SelfClosing()
        {
            PopupManager.instance.CloseUI(ID);
        }
    }
}
