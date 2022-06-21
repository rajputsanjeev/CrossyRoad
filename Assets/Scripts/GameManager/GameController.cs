using Photon.Pun;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using CrossyRoad;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System;
using System.Collections;
using UnityEngine.UI;

namespace Crossyroad
{
    public class GameController : RaiseEventListener<Player> , IGameManager
    {
        public static Action<IGameManager> OnInit;

         private GamePlayView m_View;

        [SerializeField] TileManager tileManager;
        [SerializeField] private List<PlayerRandomPosition> spawnPosition = new List<PlayerRandomPosition>();
        [SerializeField] public CameraMovement cameraMovement;
        [SerializeField] public List<Transform> playerTransform = new List<Transform>();
        [SerializeField] private int spawnCount;
        public PlayerInstance[] players;
        private Player[] punPlayersAll;
      
        [Header("Leaderboard:")]
        public LeaderboardItem[] leaderboardItems;

        #region Unity Calls
        protected override void Init()
        {
            OnInit?.Invoke(this);
            cameraMovement.enabled = false;
            m_View = (GamePlayView)Prefab;
            players = GeneratePlayerInstances(true);
            RaiseEvent(RaiseEventType.ACKNOWLEDGE_SCENE_LOAD, Photon.Realtime.ReceiverGroup.All);

            // Cache current player list:
            punPlayersAll = PhotonNetwork.PlayerList;
        }

        private PlayerInstance[] GeneratePlayerInstances(bool fresh)
        {
            PlayerInstance[] p = players;

            if (players.Length == 0 || fresh)
            {
                p = new PlayerInstance[punPlayersAll.Length];

                for (int i = 0; i < p.Length; i++)
                {
                    p[i] = new PlayerInstance(punPlayersAll[i].ActorNumber, punPlayersAll[i].NickName , (int)punPlayersAll[i].CustomProperties["Score"],
                        punPlayersAll[i]);
                }
            }
            return p;
        }
        public PlayerInstance GetPlayerInstance(int playerID)
        {
            // Look in human player list:
            for (int i = 0; i < players.Length; i++)
            {
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

        private void Update()
        {
            if (playerTransform.Count > 0)
                PlayerTransforms();
        }

        #endregion

        #region SpawnPlayer
        private void StartGame()
        {
            Spawn();
        }
        private void Spawn()
        {
            RaiseEvent(RaiseEventType.PLAYER_SPAWN, Photon.Realtime.ReceiverGroup.All);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Chicken"), GetTransform().position, Quaternion.identity);
        }

        public void SetTileManager()
        {
            //tileManager.playerTransform = prefab.transform;
            //tileManager.enabled = true;
            //tileManager.Init();
        }
        #endregion

        #region Get and set PlayerTransform
        private void GetAllPlayerTransform()
        {
            PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();

            foreach (var item in players)
            {
                playerTransform.Add(item.transform);
            }
            m_View.countdownTimer.Play();
        }

        private void PlayerTransforms()
        {
            Transform minTransform = playerTransform[0];
            for (int i = 0; i < playerTransform.Count; i++)
            {
                if (minTransform.position.z > playerTransform[i].position.z)
                {
                    minTransform = playerTransform[i];
                }
            }
            cameraMovement.playerTransform = minTransform;
        }

        private Transform GetTransform()
        {
            int turnId = int.Parse(PhotonNetwork.LocalPlayer.CustomProperties["PositionCount"].ToString());
            return spawnPosition[turnId].position;
        }
        #endregion

        #region PhotonListner and RiseEvent
        public override void OnPhotonEventExecuted(Player data)
        {
            SetPlayerInstance(data);
        }

        public void SetPlayerInstance(Player forPlayer)
        {
            PlayerInstance p = GetPlayerInstance(forPlayer.NickName);
            p.SetStats((int)forPlayer.CustomProperties["Score"]);
        }

        protected override void OnRaiseEventReceived(EventData eventData)
        {
            base.OnRaiseEventReceived(eventData);
            switch (raiseEventType)
            {
                case RaiseEventType.START_GAME:
                     StartGame();
                    break;
                case RaiseEventType.PLAYER_SPAWN:
                    PlayerSpawn();
                    break;
                case RaiseEventType.ALL_PLAYER_SPAWN:
                    GetAllPlayerTransform();
                    break;
            }
        }
        #endregion

        #region All player Spawn
        private void PlayerSpawn()
        {
            spawnCount++;
            if(spawnCount == PhotonNetwork.CurrentRoom.MaxPlayers && PhotonNetwork.IsMasterClient)
            {
                Debug.Log("ALL_PLAYER_SPAWN");
                RaiseEvent(RaiseEventType.ALL_PLAYER_SPAWN, Photon.Realtime.ReceiverGroup.All);
                spawnCount = 0;
            }
        }

        #endregion

        #region  Game event 

        public void OnCountdownCompleted()
        {
            cameraMovement.enabled = true;
        }

        public void UpdateScore()
        {

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

    }
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

