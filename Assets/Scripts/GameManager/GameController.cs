using Photon.Pun;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using CrossyRoad;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace Crossyroad
{
    public class GameController : RaiseEventListener<PlayerStatus>
    {
        [SerializeField] GamePlayView m_View;
        [SerializeField] TileManager tileManager;
        [SerializeField] private List<PlayerRandomPosition> spawnPosition = new List<PlayerRandomPosition>();
        [SerializeField] public CameraMovement cameraMovement;
        [SerializeField] public List<Transform> playerTransform = new List<Transform>();
        [SerializeField] private int spawnCount;

        #region Unity Calls
        protected override void Init()
        {
            m_View = (GamePlayView)Prefab;
            RaiseEvent(RaiseEventType.ACKNOWLEDGE_SCENE_LOAD, Photon.Realtime.ReceiverGroup.All);
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
            GameObject prefab = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Chicken"), GetTransform().position, Quaternion.identity);
            tileManager.playerTransform = prefab.transform;
            tileManager.enabled = true;
            tileManager.Init();
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
        public override void OnPhotonEventExecuted(PlayerStatus data)
        {

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
    }
    [System.Serializable]

    public class PlayerRandomPosition
    {
        public Transform position;
    }
}

