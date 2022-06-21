using UnityEngine;
using System.Collections;


namespace ObserverPattern
{
    //Events
    public abstract class PlatformEvents
    {
        public abstract float GetJumpForce();
    }


    public class AddToPool : PlatformEvents
    {
        public override float GetJumpForce()
        {
            return 30f;
        }
    }
}