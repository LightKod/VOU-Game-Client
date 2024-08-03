using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class L2DModelParameter : MonoBehaviour
    {
        public CubismModel model;
        public Dictionary<string, CubismParameter> parameters = new();

        private L2DModelAnimation[] animations;
        protected virtual void Awake()
        {
            GenerateParameters();
            animations = GetComponents<L2DModelAnimation>();
        }

        private void LateUpdate()
        {
            foreach (var anim in animations)
            {
                anim.UpdateModel();
            }
        }

        protected void GenerateParameters()
        {
            foreach (CubismParameter parameter in model.Parameters)
            {
                parameters.Add(parameter.Id, parameter);
            }
        }

        public void SetParam(string id, float value, CubismParameterBlendMode blendMode = CubismParameterBlendMode.Override)
        {
            parameters[id].BlendToValue(blendMode, value);
        }

        public float GetParam(string id)
        {
            return parameters[id].Value;
        }
    }
}
