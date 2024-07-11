using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Owlet
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T instance;
        protected bool init = true;
        [SerializeField] bool persist = false;
        private void Awake()
        {
            if (instance == null) instance = this as T;
            else
            {
                Destroy(gameObject);
                init = false;
                return;
            }
            if (persist)
            {
                transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
            Init();
            init = true;
        }

        protected virtual void Init()
        {

        }
    }
}
