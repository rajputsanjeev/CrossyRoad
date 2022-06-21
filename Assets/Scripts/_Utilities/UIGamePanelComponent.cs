using UnityEngine;

namespace Crossyroad
{
    public class UIGamePanelComponent : UIPanelComponent
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


