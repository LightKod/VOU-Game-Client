using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using Owlet.UI.Popups;
using DG.Tweening;
namespace Owlet.UI
{
    public class PopupManager : Singleton<PopupManager>
    {
        //[SerializeField] AssetLabelReference uiLabelRef;

        [SerializeField] Transform[] layers;

        Dictionary<string, Popup> cachedObjects = new();
        Dictionary<string, float> countdownDictionary = new();
        HashSet<string> tryingToOpen = new();
        HashSet<string> activePopup = new();

        [SerializeField] CanvasGroup backdrop;

        public static Action onUIOpened;
        public static Action onUIClosedAll;

        private void Update()
        {
            //CountCooldown();
        }

        public async UniTask<T> OpenUI<T>(string key, int layer = 0, bool backdropInteractable = true) where T : Popup
        {
            if (tryingToOpen.Contains(key)) return null;

            if (layer >= layers.Length) return null;

            backdrop.interactable = backdropInteractable;

            Transform parent = layers[layer];
            //Helper.Log($"Opening UI: {key}");
            //If UI is already opened
            if (cachedObjects.ContainsKey(key))
            {
                cachedObjects[key].EnableUI();
                cachedObjects[key].transform.SetParent(parent);
                cachedObjects[key].transform.SetAsLastSibling();
                //Helper.Log($"Containing UI: {key}");

                if (countdownDictionary.ContainsKey(key)) countdownDictionary.Remove(key);
                activePopup.Add(key);
                SetBackDropState(true);
                //if (triggerEvent)
                onUIOpened?.Invoke();
                return cachedObjects[key] as T;
            }
            //Helper.Log($"Open New UI: {key}");
            tryingToOpen.Add(key);
            Popup uiPrefab = await AddressableLoader.Load<Popup>(key);
            /*AsyncOperationHandle<GameObject> opHandle = Addressables.LoadAssetAsync<GameObject>(key);
            await opHandle.Task;*/
            if (uiPrefab != null)
            {
                Popup ui = Instantiate(uiPrefab, parent);
                ui.EnableUI();
                cachedObjects.Add(key, ui);

                if (countdownDictionary.ContainsKey(key)) countdownDictionary.Remove(key);
                activePopup.Add(key);
                onUIOpened?.Invoke();
                SetBackDropState(true);

                tryingToOpen.Remove(key);
                return ui as T;
            }
            else
            {
                Debug.Log(">> Error loading UI: " + key);
                return null;
            }
        }

        public void CloseUI(string key)
        {
            if (!cachedObjects.ContainsKey(key))
            {
                Debug.Log("UI is not opened");
                return;
            }

            activePopup.Remove(key);
            if (activePopup.Count == 0)
            {
                SetBackDropState(false);
                onUIClosedAll?.Invoke();

            }

            cachedObjects[key].DisableUI();
            countdownDictionary.TryAdd(key, 5f);
        }

        public void CloseAll()
        {
            foreach(var id in activePopup)
            {
                CloseUI(id);
            }
        }

        void CountCooldown()
        {
            List<string> keys = new List<string>(countdownDictionary.Keys);
            foreach (var key in keys)
            {
                countdownDictionary[key] -= Time.deltaTime;

                if (countdownDictionary[key] <= 0)
                {
                    ReleaseUI(key);
                }
            }
        }
        void ReleaseUI(string key)
        {
            Destroy(cachedObjects[key]);
            Addressables.Release(key);

            countdownDictionary.Remove(key);
            cachedObjects.Remove(key);
            //Debug.Log("Released: " + key);
        }
        public void OnBackdropClick()
        {
            if (activePopup.Count == 0) return;
            foreach (string key in activePopup)
            {
                cachedObjects[key].DisableUI();
            }
            activePopup.Clear();
            SetBackDropState(false);
            onUIClosedAll?.Invoke();
        }

        void SetBackDropState(bool on)
        {
            DOTween.Kill(backdrop.gameObject);
            if (on)
            {
                Sequence toggleSequence = DOTween.Sequence()
                                .AppendCallback(() => { backdrop.gameObject.SetActive(true); backdrop.blocksRaycasts = true; })
                                .Append(backdrop.DOFade(1, 0.3f))
                                .SetTarget(backdrop.gameObject);
            }
            else
            {
                Sequence toggleSequence = DOTween.Sequence()
                               .Append(backdrop.DOFade(0, 0.3f))
                               .AppendCallback(() => { backdrop.gameObject.SetActive(false); backdrop.blocksRaycasts = false; })
                               .SetTarget(backdrop.gameObject);
            }
        }
    }
}