using UnityEngine;
using ObserverPattern;
using Photon.Pun;
using CrossyRoard;
using System;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using System.IO;

namespace CrossyRoad.TileController.MultiPlayer
{
    public class TileManager : RaiseEventListener
    {
        public Transform cameraTransform;
        public TileGeneration tileGeneration;
        public Transform spawnPoint;
        public Transform tileParent;
        public Transform obstacelParent;
        private ObserverHandler subject = new ObserverHandler();
        [SerializeField] private float maxDistance = 50f;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        #region Multiplayer
        public void SpawnTiles()
        {
            float spawnPointDistance = Vector3.Distance(spawnPoint.position, cameraTransform.position);

            Debug.Log("spawnPointDistance " + spawnPointDistance);
            while(spawnPointDistance < maxDistance)
            {
                //Get Object from scripitable object
                Tile tile = tileGeneration.GetTile<Tile>();

                //Activity
                bool force = true;

                //Position 
                Vector3 postion = spawnPoint.position;
                Vector3 scale = new Vector3(3, 1, 3);
                spawnPoint.position = new Vector3(0, 0, spawnPoint.position.z + 3);
                spawnPointDistance += 3;
                Debug.Log("spawnPointDistance " + spawnPointDistance);
                //Instantiate data
                object[] instantiationData = { force , postion,scale};

                //Spawn Object
                GameObject tileObject = PhotonNetwork.InstantiateRoomObject(Path.Combine("Platform", tile.tileObject.name), Vector3.zero, Quaternion.identity, 0, instantiationData);

                StaticPlatform box = new StaticPlatform(tileObject, cameraTransform, tileObject.transform);
                subject.AddObserver(box);
            }
        }


        public void NotifyTile(float maxDistance)
        {
             subject.Notify();
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
                case RaiseEventType.PLAYER_MOVE:
                    Debug.Log(" PLAYER_MOVE");
                    SpawnTiles();
                    NotifyTile(maxDistance);
                    break;
                case RaiseEventType.CAMERA_SPAWN:
                    cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
                    SpawnTiles();
                    break;

            }
        }
        #endregion

    }

}
