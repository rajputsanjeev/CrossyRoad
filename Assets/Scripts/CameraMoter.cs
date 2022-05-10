using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crossyroad 
{
    public class CameraMoter
    {
        public bool moving => playerMovement.IsMoving;
        private Transform playerTransform;
        private Transform cameraTransform;
        private PlayerMovement playerMovement;
        public CameraSetting cameraSetting;
        private Vector3 offset;
        private Vector3 initialOffset;

        public CameraMoter(Transform playerTransform, Transform cameraTransform , CameraSetting cameraSetting , PlayerMovement playerMovementScript , Vector3 initialOffset)
        {
            this.playerTransform = playerTransform;
            this.cameraTransform = cameraTransform;
            this.cameraSetting = cameraSetting; 
            this.playerMovement = playerMovementScript;
            this.initialOffset = initialOffset; 
            offset = this.initialOffset;
        }

        public void InitPos()
        {
            cameraTransform.position = initialOffset;
            Vector3 playerPosition = playerTransform.position;
            cameraTransform.position =playerPosition + offset;
        }

        public void UpdateCameraMotion()
        {
            if (moving)
            {
                //Debug.Log("Moving");
                Vector3 playerPosition = playerTransform.position;
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, playerPosition + offset , Time.deltaTime);

                // Increase z over time if moving.
                //offset.z += cameraSetting.speedIncrementZ * Time.deltaTime;

            }
        }
    }


}

