using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crossyroad
{
    public class Water : Platform
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            Init();
        }

        void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter();
            // When collide with player, flatten it!
            if (other.gameObject.tag == "Player")
            {
                Vector3 scale = other.gameObject.transform.localScale;
                //other.gameObject.SendMessage("GameOver");
                PlayerMovement.gameOverEvent?.Invoke();
            }
        }
    }

}
