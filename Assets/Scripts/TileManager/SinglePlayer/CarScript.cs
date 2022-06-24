using UnityEngine;
using CrossyRoard;
using Photon.Pun;

namespace CrossyRoad.TileController
{
    public class CarScript : Movement
    {

        protected override void Update()
        {
            base.Update();
        }

      
        void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter();
            // When collide with player, flatten it!
            if (other.gameObject.tag == "Player")
            {
                Vector3 scale = other.gameObject.transform.localScale;
                other.gameObject.transform.localScale = new Vector3(scale.x, scale.y * 0.1f, scale.z);
                MyEventArgs.UIEvents.OnPlayerDie.Dispatch(PlayerStatus.ON_PLAYER_DIE);
            }
        }
    }
}

