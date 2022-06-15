using Photon.Pun;
using System.IO;
using UnityEngine;

namespace Crossyroad
{
    public class GameController : MonoBehaviour
    {
        public static GameController instance;

        [SerializeField]
        public CameraMovement cameraMovement;

        private void Awake()
        {
            if(instance == null)
                instance = this;

            Spawn();
        }

        private void Spawn()
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Chicken"), new Vector3(0,2,15), Quaternion.identity);
        }
    }
}

