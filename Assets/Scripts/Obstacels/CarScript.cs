using UnityEngine;
using System.Collections;

namespace Crossyroad 
{
    public class CarScript : Movement
    {
        private Rigidbody playerBody;

        protected override void Update()
        {
            base.Update();
        }

        void OnTriggerEnter(Collider other)
        {
            // When collide with player, flatten it!
            if (other.gameObject.tag == "Player")
            {
                //Vector3 scale = other.gameObject.transform.localScale;
                //other.gameObject.transform.localScale = new Vector3(scale.x, scale.y * 0.1f, scale.z);
                ////other.gameObject.SendMessage("GameOver");
                //PlayerMovement.gameOverEvent?.Invoke();
            }
        }
    }

}

