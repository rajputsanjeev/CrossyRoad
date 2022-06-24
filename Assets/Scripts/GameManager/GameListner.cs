using CrossyRoad;
using CrossyRoad.TileController.GameManager;
using UnityEngine;

namespace CrossyRoad.IGameListner 
{
    public class GameListner : MonoBehaviour
    {
        protected IGameManager gameEventListener;

        private void Awake()
        {
            GameController.OnInit += Init;
        }

        private void OnDestroy()
        {
            GameController.OnInit -= Init;
        }

        protected void Init(IGameManager gameEventListener)
        {
            Debug.Log("Set");
            this.gameEventListener = gameEventListener;
        }
    }

}

