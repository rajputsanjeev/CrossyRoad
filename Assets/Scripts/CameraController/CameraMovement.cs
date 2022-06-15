using UnityEngine;
using System.Collections;

namespace Crossyroad 
{
    public class CameraMovement: MonoBehaviour
    {
        protected CameraMoter cameraMoter;
        public Transform playerTransform;
        public CameraSetting cameraSetting;
        public Vector3 initialOffset;

        public void Init()
        {
            cameraMoter = new CameraMoter(playerTransform, transform, cameraSetting, playerTransform.gameObject.GetComponent<PlayerMovement>(), initialOffset);
            cameraMoter.InitPos();

            if (GameController.instance != null)
                GameController.instance.cameraMovement = this;
        }

        public void LateUpdate()
        {
            if(playerTransform != null)
               cameraMoter.UpdateLateUpdate();
        }
    }

}


