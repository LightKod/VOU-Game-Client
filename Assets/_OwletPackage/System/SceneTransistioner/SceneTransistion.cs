using DG.Tweening;
using Lean.Gui;
using Owlet.Systems.SaveLoad;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Owlet.Systems.SceneTransistions
{
    public class SceneTransistion : Singleton<SceneTransistion>
    {
        bool isTransistioning = false;
        [SerializeField] LeanToggle toggle;
        public static float animTime = 0.5f; //DO NOT CHANGE

        public static Action<float> onProgressUpdate;

        protected override void Init()
        {
            base.Init();
        }


        private void OnApplicationQuit()
        {
            DisableLoadingScreen();
        }

        public void DisableLoadingScreen()
        {
            toggle.TurnOff();
        }
        public void EnableLoadingScreen()
        {
            toggle.TurnOn();
        }


        public void ChangeScene(string sceneName)
        {
            if (isTransistioning) return;
            StartCoroutine(ChangeSceneCoroutine(sceneName));
        }

        IEnumerator ChangeSceneCoroutine(string sceneName)
        {
            isTransistioning = true;

            EnableLoadingScreen();
            yield return new WaitForSeconds(1.5f);
            yield return LoadScene(sceneName);
            yield return new WaitForSeconds(1.5f);
            //ShowGameplayScene();

            isTransistioning = false;
        }


        IEnumerator LoadScene(string scene)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
            asyncLoad.allowSceneActivation = false;
            float time = 0;
            float progress = 0;
            float minLoadTime = 1f;
            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                time += Time.deltaTime;
                time = Mathf.Clamp(time, 0, minLoadTime);
                progress = time / minLoadTime / 4;
                progress += asyncLoad.progress / 0.9f / 4 * 3;
                onProgressUpdate?.Invoke(progress);
                if (progress >= 1f)
                {
                    asyncLoad.allowSceneActivation = true;
                }

                yield return null;
            }
        }


    }
}
