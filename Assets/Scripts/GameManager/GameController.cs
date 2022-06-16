using Photon.Pun;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
namespace Crossyroad
{
    public class GameController : MonoBehaviour
    {
        public static GameController instance;
        [SerializeField] TileManager tileManager;
        [SerializeField] private List<PlayerRandomPosition> spawnPosition = new List<PlayerRandomPosition>();
        [SerializeField] public CameraMovement cameraMovement;

        private void Awake()
        {
            if(instance == null)
                instance = this;

            Spawn();
        }

        private void Spawn()
        {
        GameObject prefab = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Chicken"), GetTransform().position , Quaternion.identity);
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
    }
    [System.Serializable]

    public class PlayerRandomPosition
    {
        public bool IsUserd;
        public string name;
        public Transform position;
    }
}

