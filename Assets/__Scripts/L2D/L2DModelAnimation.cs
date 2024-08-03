using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    [RequireComponent(typeof(L2DModelParameter))]
    public abstract class L2DModelAnimation : MonoBehaviour
    {
        protected L2DModelParameter parameters;
        protected virtual void Start()
        {
            parameters = GetComponent<L2DModelParameter>();
        }

        public abstract void UpdateModel();
    }
}
