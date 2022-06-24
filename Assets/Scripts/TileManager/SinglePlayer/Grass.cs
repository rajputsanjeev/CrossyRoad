using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrossyRoad
{
    public class Grass : Platform
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

        public void SetPlayerTransform(Transform playerTransform, IObjectPool objectPool)
        {
            this.playerTransform = playerTransform;
            this.objectPoolLisner = objectPool;
        }
        protected override void OnEnable()
        {
            GrassObject();
        }

        public void GrassObject()
        {
            generatedObjects = new List<GameObject>();

            if (generatedObjects.Count == 0)
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
                            GameObject prefab = generatedObjects[Random.Range(0, generatedObjects.Count)];
                            GameObject o = (GameObject)Instantiate(prefab, relative ? transform.position + new Vector3(x, y, z) : new Vector3(x, y, z), Quaternion.identity);
                            o.transform.SetParent(gameObject.transform);
                            generatedObjects.Add(o);
                        }
                    }
                }
            }
        }

        protected override void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);
        }
    }
}
