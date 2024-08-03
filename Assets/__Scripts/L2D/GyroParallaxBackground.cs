using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class GyroParallaxBackground : MonoBehaviour
    {
        [SerializeField]
        private float shiftModofier = 1f;

        private Gyroscope gyro;

        void Start()
        {
            gyro = Input.gyro;
            gyro.enabled = true;
        }

        void Update()
        {
            transform.Translate((float)System.Math.Round(gyro.rotationRateUnbiased.y, 1) * shiftModofier, 0f, 0f);
        }
    }
}
