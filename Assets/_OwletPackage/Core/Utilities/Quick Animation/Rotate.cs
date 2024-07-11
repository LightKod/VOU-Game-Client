using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Owlet.Animations
{
    public class Rotate : MonoBehaviour
    {
        [SerializeField] Vector3 axis;
        private void Update()
        {
            transform.Rotate(axis * Time.deltaTime);
        }
    }
}
