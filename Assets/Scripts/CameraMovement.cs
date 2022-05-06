using UnityEngine;
using System.Collections;

namespace Crossyroad 
{
    public class CameraMovement: MonoBehaviour
    {
        public CameraMoter cameraMoter;
        public Transform playerTransform;
        public CameraSetting cameraSetting;

        public void Start()
        {
            cameraMoter = new CameraMoter(playerTransform, transform, cameraSetting, playerTransform.gameObject.GetComponent<PlayerMovement>());
        }

        public void Update()
        {
            cameraMoter.UpdateCameraMotion();
        }
    }

}


