using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Owlet
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasFindUICamera : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Canvas>().worldCamera = GameObject.Find("UI Camera").GetComponent<Camera>();
            SceneManager.activeSceneChanged += Find;
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= Find;
        }

        [Button]
        void Find()
        {
            GetComponent<Canvas>().worldCamera = GameObject.Find("UI Camera").GetComponent<Camera>();
        }

        void Find(Scene scene, Scene scene1)
        {
            GetComponent<Canvas>().worldCamera = GameObject.Find("UI Camera").GetComponent<Camera>();
        }
    }
}
