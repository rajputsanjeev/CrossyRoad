using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Crossyroad
{
    [System.Serializable]
    public class TileMotor
    {
        private Transform playerTransform;
        private Transform spawnPoint;
        private Transform parent;
        private TileGeneration tileGeneration;
        public List<GameObject> objectPool = new List<GameObject>();
        
        public TileMotor(Transform transform , TileGeneration tileGeneration , Transform spawnPoint , Transform parent)
        {
            this.playerTransform = transform;
            this.tileGeneration = tileGeneration;
            this.spawnPoint = spawnPoint;
            this.parent = parent;
        }

        public void SpawnTileAtZero(IObjectPool IobjectPool ,float minDistance , float maxDistance)
        {
            Tile tile = tileGeneration.GetTile<Tile>();
            GameObject tileObject = tileGeneration.SpawnTile(tile);
            tileObject.transform.SetParent(parent);
            tileObject.transform.localScale = new Vector3(3, 1, 3);
            objectPool.Add(tileObject);
            tileObject.SetActive(false);

            if (tile.tileType != TileType.GRASS)
            {
                tileObject.GetComponent<Platform>().SetPlayerTransform(playerTransform, IobjectPool);
            }
            else
            {
                tileObject.GetComponent<Grass>().SetPlayerTransform(playerTransform, IobjectPool);
            }
        }

        #region UnUsed Code
        public void SpawnTile(IObjectPool IobjectPool, float minDistance, float maxDistance)
        {
            //Debug.Log("Distance " + Vector3.Distance(spawnPoint.position, playerTransform.position));
            Tile tile = tileGeneration.GetTile<Tile>();
            GameObject tileObject = tileGeneration.SpawnTile(tile);
            tileObject.transform.SetParent(parent);
            tileObject.transform.position = spawnPoint.position;
            tileObject.transform.localScale = new Vector3(3, 1, 3);
            spawnPoint.position = new Vector3(playerTransform.position.x, 0, spawnPoint.position.z + 3);

            if (Vector3.Distance(spawnPoint.position, playerTransform.position) > minDistance && Vector3.Distance(spawnPoint.position, playerTransform.position) < maxDistance)
            {
                tileObject.SetActive(true);
                if (tile.tileType != TileType.GRASS)
                {
                    tileObject.GetComponent<Platform>().SetPlayerTransform(playerTransform, IobjectPool);
                    tileObject.GetComponent<Platform>().Init();
                }
                else
                {
                    tileObject.GetComponent<Grass>().SetPlayerTransform(playerTransform, IobjectPool);
                    tileObject.GetComponent<Grass>().Init();
                }
            }
            else
            {
                tileObject.SetActive(false);
            }

        }
        #endregion

        public void SpawnTileFromPool(float minDistance, float maxDistance)
        {
            if (Vector3.Distance(spawnPoint.position, playerTransform.position) > minDistance && Vector3.Distance(spawnPoint.position, playerTransform.position) < maxDistance)
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
                    Debug.Log("spawnPoint.position z " + spawnPoint.position.z);
                }
            }
        }
    }
}

