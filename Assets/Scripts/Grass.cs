using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crossyroad
{
    public class Grass : MonoBehaviour
    {
        private IObjectPool objectPoolLisner;
        private Transform playerTransform;
        public Vector3 minPosition;
        public Vector3 maxPosition;
        public Vector3 gridSize = new Vector3(1, 1, 3);

        public float density = 0.12f;
        public bool relative = true;
        public bool destroyWhenDestroyed = true;

        private List<GameObject> generatedObjects;
        public List<GameObject> tileList = new List<GameObject>();

        public void SetPlayerTransform(Transform playerTransform, IObjectPool objectPool)
        {
            this.playerTransform = playerTransform;
            this.objectPoolLisner = objectPool;
        }

        public void Init()
        {
            generatedObjects = new List<GameObject>();

            if (tileList.Count == 0)
                return;

            for (var x = minPosition.x; x <= maxPosition.x; x += gridSize.x)
            {
                for (var y = minPosition.y; y <= maxPosition.y; y += gridSize.y)
                {
                    for (var z = minPosition.z; z <= maxPosition.z; z += gridSize.z)
                    {
                        bool generate = Random.value < density;
                        if (generate)
                        {
                            GameObject prefab = tileList[Random.Range(0, tileList.Count)];
                            var o = (GameObject)Instantiate(prefab, relative ? transform.position + new Vector3(x, y, z) : new Vector3(x, y, z), Quaternion.identity);
                            generatedObjects.Add(o);
                        }
                    }
                }
            }
        }

        private void OnDisable()
        {
            
        }
     
    }

}
