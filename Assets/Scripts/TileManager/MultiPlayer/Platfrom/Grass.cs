using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CrossyRoad.TileController.MultiPlayer.Platform
{
    public class Grass : MonoBehaviour
    {
        public Vector3 minPosition;
        public Vector3 maxPosition;
        public Vector3 gridSize = new Vector3(1, 1, 3);

        public float density = 0.12f;
        public bool relative = true;
        public bool destroyWhenDestroyed = true;

        private List<GameObject> generatedObjects = new List<GameObject>();

        public PhotonView photonView;
        public List<ObstacelType> obstacelTypes;

        protected  void Awake()
        {
           photonView = GetComponent<PhotonView>();

            if (photonView.InstantiationData != null)
            {
                Debug.Log("Active " + (bool)photonView.InstantiationData[0]);
                gameObject.SetActive((bool)photonView.InstantiationData[0]);
                transform.position = (Vector3)photonView.InstantiationData[1];
                transform.localScale = (Vector3)photonView.InstantiationData[2];
            }
        }

        public void SetObstacelType(List<ObstacelType> obstacelTypes, TileType tileType)
        {
            this.obstacelTypes = obstacelTypes;
            GrassObject(tileType, obstacelTypes);
        }


        public void GrassObject(TileType tileType, List<ObstacelType> tileList)
        {

            for (var x = minPosition.x; x <= maxPosition.x; x += gridSize.x)
            {
                for (var y = minPosition.y; y <= maxPosition.y; y += gridSize.y)
                {
                    for (var z = minPosition.z; z <= maxPosition.z; z += gridSize.z)
                    {
                        bool generate = Random.value < density;
                        if (generate)
                        {
                            GameObject obstacel = PhotonNetwork.InstantiateRoomObject(Path.Combine(Path.Combine("Obstacel", tileType.ToString().ToLower()), tileList[0].obstacelName), relative ? transform.position + new Vector3(x, y, z) : new Vector3(x, y, z), Quaternion.identity);
                            obstacel.transform.SetParent(gameObject.transform);
                        }
                    }
                }
            }
        }
    }
}
