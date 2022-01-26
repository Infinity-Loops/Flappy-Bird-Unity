using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DotGears.Flappy
{
    public static class Touch
    {
        public static bool GetTouchDown()
        {
            if (Input.touchCount > 0)
            {
                var Touch = Input.GetTouch(0);
                if (Touch.type == TouchType.Direct && Touch.tapCount > 0 || Touch.phase == TouchPhase.Began)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}