using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Crossyroad
{
    public class TileMotor
    {
        private Transform playerTransform;
        public Transform spawnPoint;
        private TileGeneration tileGeneration;
        public List<GameObject> objectPool = new List<GameObject>();
        
        public TileMotor(Transform transform , TileGeneration tileGeneration , Transform spawnPoint)
        {
            this.playerTransform = transform;
            this.tileGeneration = tileGeneration;
            this.spawnPoint = spawnPoint;
        }

        public void SpawnTile(IObjectPool IobjectPool)
        {
            Debug.Log("objectPool.Count " + objectPool.Count);
            if(objectPool.Count > 0)
            {
                if (Vector3.Distance(spawnPoint.position, playerTransform.position) > 10f && Vector3.Distance(spawnPoint.position, playerTransform.position) < 100f)
                {
                    Tile tile = tileGeneration.GetTile<Tile>();
                    GameObject tileObject = tileGeneration.SpawnTile(tile);
                    tileObject.transform.position = spawnPoint.position;
                    tileObject.transform.localScale = new Vector3(1, 1, 3);
                    spawnPoint.position = new Vector3(0, 0, spawnPoint.position.z + 3);
                    tileObject.GetComponent<Platform>().SetPlayerTransform(playerTransform, IobjectPool);
                }
            }
            else
            {
                int randomTile =  Random.Range(0,objectPool.Count-1);
                GameObject tileObject = objectPool[randomTile];
                tileObject.transform.position = spawnPoint.position;
                tileObject.transform.localScale = new Vector3(1, 1, 3);
                spawnPoint.position = new Vector3(0, 0, spawnPoint.position.z + 3);
                objectPool.RemoveAt(randomTile);
            }
        }
    }
}

