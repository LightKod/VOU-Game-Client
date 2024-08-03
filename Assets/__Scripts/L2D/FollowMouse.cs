using Owlet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VOU
{
    public class FollowMouse : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                transform.position = Helper.GetMouseWorldPosition();
            }
        }
    }
}
