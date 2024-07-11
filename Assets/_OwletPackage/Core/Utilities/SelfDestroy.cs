using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Owlet
{
    public class SelfDestroy : MonoBehaviour
    {
        [SerializeField] float time = 1f;
        // Start is called before the first frame update
        private void OnEnable()
        {
            StartCoroutine(DelayDestroy());
        }

        IEnumerator DelayDestroy()
        {
            yield return new WaitForSeconds(time);
            LeanPool.Despawn(gameObject);
        }

    }
}
