using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrossyRoad.TileController.SinglePlayer.Platform
{
    public class Grass : MonoBehaviour
    {
        public Vector3 minPosition;
        public Vector3 maxPosition;
        public Vector3 gridSize = new Vector3(1, 1, 3);

        public float density = 0.12f;
        public bool relative = true;
        public bool destroyWhenDestroyed = true;

        private List<GameObject> generatedObjects;
        public List<ObstacelType> obstacelTypes;

        protected  void OnEnable()
        {
        }

        public void SetObstacelType(List<ObstacelType> obstacelTypes, TileType tileType)
        {
            this.obstacelTypes = obstacelTypes;
            GrassObject(obstacelTypes , tileType);
        }


        public void GrassObject(List<ObstacelType> obstacelTypes, TileType tileType)
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

    }
}
