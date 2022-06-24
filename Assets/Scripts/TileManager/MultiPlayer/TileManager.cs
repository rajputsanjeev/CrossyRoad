using UnityEngine;
using ObserverPattern;
using Photon.Pun;
using CrossyRoard;
using System;

namespace CrossyRoad.TileController.MultiPlayer
{
    public class TileManager : MonoBehaviour, IObjectPool
    {
        public Transform cameraTransform;
        public TileGeneration tileGeneration;
        public Transform spawnPoint;
        public Transform tileParent;
        public Transform obstacelParent;
        private TileMotor tileMotor;
        private ObserverHandler subject = new ObserverHandler();


        protected void OnEnable()
        {
            MyEventArgs.UIEvents.initTileManager.AddListener(SpawnTiles);
            MyEventArgs.UIEvents.PlayerMove.AddListener(ChangeCameraTargert);

        }

        protected void OnDisable()
        {
            MyEventArgs.UIEvents.initTileManager.RemoveListener(SpawnTiles);
            MyEventArgs.UIEvents.PlayerMove.RemoveListener(ChangeCameraTargert);

        }


        private void ChangeCameraTargert(Transform obj)
        {
            tileMotor.SpawnTileFromPool(0f, 100f);
        }


        private void SpawnTiles(bool isMaster)
        {
            if (isMaster)
            {
                InitByMasterPlayer();
            }
            else
            {
                tileMotor = new TileMotor(cameraTransform, tileGeneration, spawnPoint, tileParent, obstacelParent, subject);
            }
        }

        private void InitByMasterPlayer()
        {
            tileMotor = new TileMotor(cameraTransform, tileGeneration, spawnPoint, tileParent, obstacelParent, subject);

            for (int i = 0; i < 30; i++)
            {
                tileMotor.SpawnTileAtZeroMultiplayer(this);
            }
        }

        public void AddToPool(GameObject gameObject)
        {
            tileMotor.objectPool.Add(gameObject);
        }
    }

}
