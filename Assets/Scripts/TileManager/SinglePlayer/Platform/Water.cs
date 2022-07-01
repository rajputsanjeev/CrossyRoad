using CrossyRoard;
using UnityEngine;

namespace CrossyRoad.TileController.SinglePlayer.Platform
{
    public class Water : Platform
    {
        protected override void OnEnable()
        {
            base.OnEnable();
        }

      protected override  void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            Debug.Log("Calll");
            MyEventArgs.UIEvents.OnPlayerDie.Dispatch(PlayerStatus.ON_PLAYER_DIE);
        }
    }

}
