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
        private Dictionary<int, GameObject> lines = new Dictionary<int, GameObject>();

        public TileMotor(Transform transform , TileGeneration tileGeneration , Transform spawnPoint)
        {
            this.playerTransform = transform;
            this.tileGeneration = tileGeneration;
            this.spawnPoint = spawnPoint;
        }

        public void SpawnTile()
        {
            Debug.Log("Distance "+Vector3.Distance(spawnPoint.position, playerTransform.position));

            if(Vector3.Distance(spawnPoint.position, playerTransform.position) > 10f && Vector3.Distance(spawnPoint.position, playerTransform.position) < 50f)
            {
               for(int i = 0; i < 5; i++)
                {
                    Tile tile = tileGeneration.GetTile<Tile>();
                    GameObject gameObject = tileGeneration.SpawnTile(tile);
                    gameObject.transform.position = spawnPoint.position;
                    gameObject.transform.localScale = new Vector3(1, 1, 3);
                    spawnPoint.position = new Vector3(0,0 ,spawnPoint.position.z + 3);
                }
            }
        }
    }
}

