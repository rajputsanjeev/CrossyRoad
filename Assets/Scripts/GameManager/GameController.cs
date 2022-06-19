using Photon.Pun;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using CrossyRoad;
using Photon.Realtime;

namespace Crossyroad
{
    public class GameController : PhotonListener<PlayerStatus ,Player>
    {
        public static GameController instance;
        [SerializeField] TileManager tileManager;
        [SerializeField] private List<PlayerRandomPosition> spawnPosition = new List<PlayerRandomPosition>();
        [SerializeField] public CameraMovement cameraMovement;
        [SerializeField] public List<Transform> playerTransform = new List<Transform>();

        protected override void Awake()
        {
            if(instance == null)
                instance = this;

            Spawn();
        }

        private void Update()
        {
            if(playerTransform.Count > 0)
              MinTransform();
        }

        private void MinTransform()
        {
            Transform minTransform = playerTransform[0];
            for (int i = 0; i < playerTransform.Count; i++)
            {
                if (minTransform.position.z < playerTransform[i].position.z)
                {
                    minTransform = playerTransform[i];
                }
            }

            cameraMovement.playerTransform = minTransform;
        }

        private void Spawn()
        {
          GameObject prefab = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Chicken"), GetTransform().position , Quaternion.identity);
          playerTransform.Add(prefab.transform);
          tileManager.playerTransform = prefab.transform;
          tileManager.enabled = true;
          tileManager.Init();
        }

        private Transform GetTransform()
        {
            int randomPoint = UnityEngine.Random.Range(0, spawnPosition.Count);

            if (spawnPosition[randomPoint].IsUserd)
                GetTransform();

            spawnPosition[randomPoint].IsUserd = true;
            spawnPosition[randomPoint].name = PhotonNetwork.LocalPlayer.NickName;
            return spawnPosition[randomPoint].position;

        }

        public override void OnPhotonEventExecuted(PlayerStatus data)
        {

        }

        public override void OnPhotonEventExecuted(PlayerStatus data, Player inform)
        {

        }
    }
    [System.Serializable]

    public class PlayerRandomPosition
    {
        public bool IsUserd;
        public string name;
        public Transform position;
    }
}

