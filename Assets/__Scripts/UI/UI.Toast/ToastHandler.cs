using Cysharp.Threading.Tasks;
using Lean.Pool;
using Owlet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class ToastHandler : Singleton<ToastHandler>
    {
        [SerializeField] Transform startPos;
        [SerializeField] Transform endPos;
        ToastItem toastItemPrefab;

        public async void Show(string msg, ToastState state = ToastState.Error)
        {
            if (toastItemPrefab == null) await LoadObjects();
            var toast = LeanPool.Spawn(toastItemPrefab, transform);
            toast.SetupUI(msg, state, startPos.transform.localPosition, endPos.transform.localPosition);
        }

        async UniTask LoadObjects()
        {
            if (toastItemPrefab == null) toastItemPrefab = await AddressableLoader.Load<ToastItem>("UI_Toast");
        }
    }
}
