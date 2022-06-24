
using UnityEngine;
using System.Collections.Generic;
using System;
using CrossyRoard;
using CrossyRoad.ScoreManagerNamespace;

namespace CrossyRoad.PlayerSinglePlayer.GameManager
{
    public class GameController : MonoBehaviour
    {
     
        public ScoreManager scoreManager;

        public GameObject dieScreen;

        [SerializeField] private List<Transform> spawnPosition = new List<Transform>();

        #region Unity Calls
        protected void Init()
        {

        }

        protected void OnEnable()
        {
            MyEventArgs.UIEvents.updateScore.AddListener(UpdateScore);
            MyEventArgs.UIEvents.OnPlayerDie.AddListener(OnPlayerDie);
        }

        protected void OnDisable()
        {
            MyEventArgs.UIEvents.updateScore.RemoveListener(UpdateScore);
            MyEventArgs.UIEvents.OnPlayerDie.RemoveListener(OnPlayerDie);
        }

        #endregion


        #region Die Action
        public void OnDieFunction(PlayerStatus playerStatus)
        {
            switch (playerStatus) 
            {
                case PlayerStatus.ON_PLAYER_DIE:
                    Debug.Log("Dieeeeee");
                    break;
            }
        }

        public void UpdateScore(int score)
        {
            scoreManager.UpdateScore(score);
        }

        public void OnPlayerDie(PlayerStatus playerStatus)
        {
            switch (playerStatus)
            {
                case PlayerStatus.ON_PLAYER_DIE:
                    MyEventArgs.UIEvents.IsMoveAble.Dispatch(false);
                    dieScreen.SetActive(true);
                    break;
            }
        }

        public void OnClick()
        {
            SceneController.LoadScene("GameSceneNoneMultiplayerScene");
        }
        #endregion

    }
 
}

