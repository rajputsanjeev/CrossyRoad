using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverPattern;
using Photon.Pun;
using System.IO;
using ObserverPattern.SinglePlayer;

namespace CrossyRoad.TileController.SinglePlayer.Platform
{
    [System.Serializable]
    public class TileMotor
    {
        private Transform cameraTransform;
        private Transform spawnPoint;
        private Transform tileParent;
        private TileGeneration tileGeneration;
        public List<GameObject> objectPool = new List<GameObject>();
        public ObserverHandler addToObserver;

        public TileMotor(Transform transform , TileGeneration tileGeneration , Transform spawnPoint , Transform tileparent , Transform obstacelParent, ObserverHandler subject)
        {
            this.cameraTransform = transform;
            this.tileGeneration = tileGeneration;
            this.spawnPoint = spawnPoint;
            this.tileParent = tileparent;
            this.addToObserver = subject;
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

            //Platform platform = tileObject.GetComponent<Platform>();
            //platform.SetObstacelType(tile.obstacelTypes);

            if (tileObject.GetComponent<Platform>() != null)
            {
                Platform platform = tileObject.GetComponent<Platform>();
                platform.SetObstacelType(tile.obstacelTypes);
            }
            else
            {
                Grass platform = tileObject.GetComponent<Grass>();
                platform.SetObstacelType(tile.obstacelTypes, tile.tileType);
            }

            PlatformObserver platformObserver = new PlatformObserver(tileObject, cameraTransform, IobjectPool);
            addToObserver.AddObserver(platformObserver);
        }
   
        public void SpawnTileFromPool(float maxDistance)
        {
            if (Vector3.Distance(spawnPoint.position, cameraTransform.position) < maxDistance)
            {
                if (objectPool.Count > 0)
                {
                    Debug.Log("Addd to pool");
                    int randomTile = Random.Range(0, objectPool.Count - 1);
                    GameObject tileObject = objectPool[randomTile];
                    tileObject.transform.position = spawnPoint.position;
                    tileObject.transform.localScale = new Vector3(3, 1, 3);
                    spawnPoint.position = new Vector3(0, 0, spawnPoint.position.z + 3);
                    tileObject.SetActive(true);
                    objectPool.RemoveAt(randomTile);
                }
            }
        }
        #endregion

    }
}

