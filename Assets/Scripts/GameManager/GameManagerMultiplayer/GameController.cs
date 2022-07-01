using Photon.Pun;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System;
using System.Collections;
using UnityEngine.UI;
using CrossyRoad.PhotonPlayerMovement;
using CrossyRoad.PlayerInstanceNamespace;
using CrossyRoard;
using CrossyRoad.TileController.MultiPlayer.Platform;

namespace CrossyRoad.TileController.GameManager
{
    public class GameController : RaiseEventListener<Player> , IGameManager
    {
        public static new GameController Instance;
        private GamePlayView m_View;

        public static Action<IGameManager> OnInit;
        public static event Action<PlayerStatus> OnDie;

        [SerializeField] TileManager tileManager;

        [SerializeField] private List<PlayerRandomPosition> spawnPosition = new List<PlayerRandomPosition>();
        [SerializeField] private int spawnCount;

        public PlayerInstance[] players;
        private Player[] punPlayersAll;
      
        [Header("Leaderboard:")]
        public LeaderboardItem[] leaderboardItems;

        #region Unity Calls
        protected override void Init()
        {
            if(players == null)
                Instance = this;

            OnInit?.Invoke(this);

            m_View = (GamePlayView)Prefab;

            players = new PlayerInstance[0];

            //Cache current player list:
            punPlayersAll = PhotonNetwork.PlayerList;

            players = GeneratePlayerInstances(true);

            Debug.Log("Player List " + players.Length);
          
            //Inform all the player my scene is load by Acknowledge Handler
            RaiseEvent(RaiseEventType.ACKNOWLEDGE_SCENE_LOAD, ReceiverGroup.All);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
        #endregion

        #region Spawn Player
        private void SpawnCamera()
        {
            Debug.Log("Player Camera");
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.InstantiateRoomObject(Path.Combine("Camera", "CameraController"), Vector3.zero, Quaternion.Euler(37f,0,0));
                RaiseEvent(RaiseEventType.SPAWN_PLAYER, ReceiverGroup.All);
                RaiseEvent(RaiseEventType.CAMERA_SPAWN, ReceiverGroup.MasterClient);

            }
        }
        private void SpawnPlayer()
        {
            Debug.Log("Player Spawn");
           GameObject player = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Chicken"), GetTransform().position, Quaternion.identity);
            player.name = PhotonNetwork.LocalPlayer.NickName;
        }

        #endregion

        #region Get and set PlayerTransform
        private void GetAllPlayerTransform()
        {
            PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();
            m_View.countdownTimer.Play();
        }

        private Transform GetTransform()
        {
            int turnId = int.Parse(PhotonNetwork.LocalPlayer.CustomProperties["PositionCount"].ToString());
            return spawnPosition[turnId].position;
        }
        #endregion

        #region PhotonListner
        public override void OnPhotonEventExecuted(Player data)
        {
            SetPlayerInstance(data);
        }

        public void SetPlayerInstance(Player forPlayer)
        {
            PlayerInstance p = GetPlayerInstance(forPlayer.NickName);
            p.SetStats((int)forPlayer.CustomProperties["Score"]);
            UpdateBoards();
        }

        #endregion

        #region OnRaiseEventReceived
        /// <summary>
        /// OnRaiseEventReceived
        /// </summary>
        /// <param name="eventData"></param>

        protected override void OnRaiseEventReceived(EventData eventData)
        {
            base.OnRaiseEventReceived(eventData);
            switch (raiseEventType)
            {
                case RaiseEventType.SPAWN_CAMERA:
                    Debug.Log(" SPAWN_CAMERA ");
                      SpawnCamera();
                    break;
                case RaiseEventType.SPAWN_PLAYER:
                    SpawnPlayer();
                    break;
                case RaiseEventType.PLAYER_SPAWNED:
                      PlayerSpawn();
                    break;

                case RaiseEventType.ALL_PLAYER_SPAWN:
                    StartGameTimer();
                    break;
            }
        }
        #endregion

        #region Spawn all player after some time
        private void PlayerSpawn()
        {
            spawnCount++;
            if(spawnCount == PhotonNetwork.PlayerList.Length && PhotonNetwork.IsMasterClient)
            {
                RaiseEvent(RaiseEventType.ALL_PLAYER_SPAWN, ReceiverGroup.All);
                spawnCount = 0;
            }
        }
        #endregion

        #region Start Timer
        private void StartGameTimer()
        {
            m_View.countdownTimer.Play();
        }
        #endregion

        #region  Game events 
        public void OnCountdownCompleted()
        {
            Debug.Log("On Count Down Complete");
            m_View.countdownTimer.Show(false);
            MyEventArgs.UIEvents.StartGame.Dispatch(true);
        }
        #endregion

        #region UpdateleaderBoard
        public void UpdateBoards()
        {
            //if (!gm.isGameOver)
            {
                // Retrieve a sorted list of players based on scores:
                PlayerInstance[] playersSorted = SortPlayersByScore();

                // Get the kills and deaths value of all players and store them in an INT array:
                int[] otherScore = new int[playersSorted.Length];

                for (int i = 0; i < playersSorted.Length; i++)
                {
                    otherScore[i] = playersSorted[i].score;
                }

                // Scoreboard:
                // clear board first:
                foreach (Transform t in m_View.content.transform)
                {
                    Destroy(t.gameObject);
                }
                // then repopulate:
                for (int i = 0; i < playersSorted.Length; i++)
                {
                    GameObject item = Instantiate(m_View.leaderBoardPrefab, m_View.content.transform);
                    ScoreboardItem scoreboardItem = item.GetComponent<ScoreboardItem>();
                    scoreboardItem.represented = playersSorted[i];
                    scoreboardItem.scoreText.text = (otherScore[i]).ToString();
                }

                // Leaderboard:
                for (int i = 0; i < leaderboardItems.Length; i++)
                {
                    if (playersSorted.Length > i)
                    {
                        leaderboardItems[i].playerName.text = playersSorted[i].playerName;
                        leaderboardItems[i].playerName.color = leaderboardItems[i].playerName.text == PhotonNetwork.NickName ? Color.cyan : Color.white;
                        leaderboardItems[i].score.text = (otherScore[i]).ToString();
                    }
                    else
                    {
                        leaderboardItems[i].playerName.color = new Color(0.1f, 0.1f, 0.1f, 1);
                    }
                }
            }
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
        #endregion

        #region PlayerInstance
        private PlayerInstance[] GeneratePlayerInstances(bool fresh)
        {
            PlayerInstance[] p = players;

            if (players.Length == 0 || fresh)
            {
                p = new PlayerInstance[punPlayersAll.Length];

                for (int i = 0; i < p.Length; i++)
                {
                    Debug.Log("punPlayersAll[i].NickName " + punPlayersAll[i].NickName);
                    p[i] = new PlayerInstance(punPlayersAll[i].ActorNumber, punPlayersAll[i].NickName, punPlayersAll[i].IsLocal , (int)punPlayersAll[i].CustomProperties["Score"], punPlayersAll[i].IsMasterClient, punPlayersAll[i]);
                }
            }
            return p;
        }
        public PlayerInstance GetPlayerInstance(int playerID)
        {
            Debug.Log("get instance");
            // Look in human player list:
            for (int i = 0; i < players.Length; i++)
            {
                Debug.Log("players[i].playerID " + players[i].playerID);
                Debug.Log("playerID " + playerID);

                if (players[i].playerID == playerID)
                {
                    return players[i];
                }
            }
            return null;
        }

        public PlayerInstance GetPlayerInstance(string playerName)
        {
            // Look in human player list:
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].playerName == playerName)
                {
                    return players[i];
                }
            }
            return null;
        }

        public PlayerInstance[] GetPlayerList()
        {
            // Get the (human) player list:
            PlayerInstance[] tempP = players;
            return tempP;
        }

        IComparer SortPlayers()
        {
            return (IComparer)new PlayerSorter();
        }
        public PlayerInstance[] SortPlayersByScore()
        {
            // Get the full player list:
            PlayerInstance[] allPlayers = GetPlayerList();

            // ...then sort them out based on scores:
            System.Array.Sort(allPlayers, SortPlayers());
            return allPlayers;
        }
        #endregion

    [System.Serializable]
    public class PlayerRandomPosition
    {
        public Transform position;
    }

    public class PlayerSorter : IComparer
    {
        int IComparer.Compare(object a, object b)
        {
            int p1 = ((PlayerInstance)a).score;
            int p2 = ((PlayerInstance)b).score;
            if (p1 == p2)
            {
                return 0;
            }
            else
            {
                if (p1 > p2)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
        }
    }

    [System.Serializable]
    public class LeaderboardItem
    {
        public Text playerName;
        public Text score;
    }
  }
}


