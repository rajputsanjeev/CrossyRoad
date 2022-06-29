using UnityEngine;
using ObserverPattern;
using Photon.Pun;

namespace CrossyRoad.TileController.Singleplayer
{
    public class TileManager : MonoBehaviour, IObjectPool
    {
        public Transform cameraTransform;
        public TileGeneration tileGeneration;
        public Transform spawnPoint;
        public Transform tileParent;
        public Transform obstacelParent;
        public TileMotor tileMotor;
        public ObserverHandler subject = new ObserverHandler();

        private void Awake()
        {
            InitSinglePlayer();
        }

        private void InitSinglePlayer()
        {
            tileMotor = new TileMotor(cameraTransform, tileGeneration, spawnPoint, tileParent, obstacelParent, subject);
            for (int i = 0; i < 30; i++)
            {
                tileMotor.SpawnTileAtZero(this);
            }
        }

        private void Update()
        {
            if (spawnPoint == null && cameraTransform == null)
                return;

            tileMotor.SpawnTileFromPool(100f);
        }

        public void AddToPool(GameObject gameObject)
        {
            tileMotor.objectPool.Add(gameObject);
        }
    }

}
