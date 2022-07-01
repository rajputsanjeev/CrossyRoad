using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrossyRoad.TileController.MultiPlayer.Platform
{
    public class Road : Platform
    {
       
      protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void OnCollisionEnter(Collision collision)
        {
           
            base.OnCollisionEnter(collision);
           
        }
    }
}

