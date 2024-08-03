using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class Frieren_LookAt : L2DModelAnimation
    {
        const string HEAD_X = "ParamAngleX";
        const string HEAD_Y = "ParamAngleY";

        [SerializeField] Transform target;
        [SerializeField] Transform head;

        public override void UpdateModel()
        {
            FollowTarget();
        }

        void FollowTarget()
        {
            Vector2 direction = (target.transform.position - head.transform.position).normalized;
            parameters.SetParam(HEAD_X, direction.x * 10f);
            parameters.SetParam(HEAD_Y, direction.y * 10f);
        }
    }
}
