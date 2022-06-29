using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverPattern;
using Photon.Pun;
using System.IO;

namespace CrossyRoad
{
    [System.Serializable]
    public class TileMotor
    {
        private Transform cameraTransform;
        private Transform spawnPoint;
        private Transform tileParent;
        private Transform obstacelParent;
        private TileGeneration tileGeneration;
        public List<GameObject> objectPool = new List<GameObject>();
        public ObserverHandler subject;

        public TileMotor(Transform transform , TileGeneration tileGeneration , Transform spawnPoint , Transform tileparent , Transform obstacelParent, ObserverHandler subject)
        {
            this.cameraTransform = transform;
            this.tileGeneration = tileGeneration;
            this.spawnPoint = spawnPoint;
            this.tileParent = tileparent;
            this.obstacelParent = obstacelParent;
            this.subject = subject;
        }

        #region Spawn Player For single player

        public void SpawnTileAtZero(IObjectPool IobjectPool)
        {
            //Get Object from scripitable object
            Tile tile = tileGeneration.GetTile<Tile>();

            //Spawn Object
            GameObject tileObject = tileGeneration.SpawnTile(tile.tileObject);

            // Set transform
            tileObject.transform.SetParent(tileParent);
            tileObject.transform.localScale = new Vector3(3, 1, 3);

            //Add to pool
            objectPool.Add(tileObject);

            // Initilie it inactive
            tileObject.SetActive(false);

            List<GameObject> tileObstacal = tileGeneration.GetTileObstacal(tile.tileList , obstacelParent);

            Platform platform = tileObject.GetComponentInChildren<Platform>();
            platform.SetObstacel(tileObstacal);

            StaticPlatform box = new StaticPlatform(tileObject, cameraTransform, tileObject.transform, IobjectPool);
            subject.AddObserver(box);

        }
   
        public void SpawnTileFromPool(float maxDistance)
        {
            if (Vector3.Distance(spawnPoint.position, cameraTransform.position) < maxDistance)
            {
                if (objectPool.Count > 0)
                {
                    int randomTile = Random.Range(0, objectPool.Count - 1);
                    GameObject tileObject = objectPool[randomTile];
                    tileObject.transform.position = spawnPoint.position;
                    tileObject.transform.localScale = new Vector3(3, 1, 3);
                    spawnPoint.position = new Vector3(0, 0, spawnPoint.position.z + 3);
                    tileObject.SetActive(true);
                    objectPool.RemoveAt(randomTile);
                    subject.Notify();
                }
            }
        }
        #endregion

        #region Multiplayer
        public void SpawnTileAtZeroMultiplayer(IObjectPool IobjectPool)
        {
            //Get Object from scripitable object
            Tile tile = tileGeneration.GetTile<Tile>();

            //Spawn Object
            GameObject tileObject = PhotonNetwork.InstantiateRoomObject(Path.Combine("Platform", tile.tileObject.name), Vector3.zero, Quaternion.identity);

            // Set transform
            tileObject.transform.SetParent(tileParent);
            tileObject.transform.localScale = new Vector3(3, 1, 3);

            //Add to pool
             objectPool.Add(tileObject);

            // Initilie it inactive
            tileObject.SetActive(false);

            List<GameObject> tileObstacal = GetTileObstacal(tile.tileType, tile.tileList, obstacelParent);

            Platform platform = tileObject.GetComponentInChildren<Platform>();
            platform.SetObstacel(tileObstacal);

            StaticPlatform box = new StaticPlatform(tileObject, cameraTransform, tileObject.transform, IobjectPool);
            subject.AddObserver(box);

        }

        public List<GameObject> GetTileObstacal(TileType tileType, List<GameObject> tileList, Transform parent)
        {
            if (tileList.Count == 0)
            {
                return null;
            }
            Debug.Log("tile list Count " + tileList.Count);
            List<GameObject> list = new List<GameObject>();
            int random = UnityEngine.Random.Range(0, tileList.Count - 1);
            GameObject tileObstacel = PhotonNetwork.InstantiateRoomObject(Path.Combine(Path.Combine("Obstacel", tileType.ToString()), tileList[random].name), Vector3.zero, Quaternion.identity);
            tileObstacel.SetActive(false);
            tileObstacel.transform.SetParent(parent, false);
            list.Add(tileObstacel);
            return list;
        }
        #endregion

    }
}

