using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crossyroad 
{
    public class CameraMoter
    {
        public bool moving = false;
        private Transform playerTransform;
        private Transform cameraTransform;
        private PlayerMovement playerMovement;
        public CameraSetting cameraSetting;
        private Vector3 offset;
        private Vector3 initialOffset;

        public CameraMoter(Transform playerTransform, Transform cameraTransform , CameraSetting cameraSetting , PlayerMovement playerMovementScript)
        {
            this.playerTransform = playerTransform;
            this.cameraTransform = cameraTransform;
            this.cameraSetting = cameraSetting; 
            this.playerMovement = playerMovementScript;
            initialOffset = new Vector3(2.5f, 10.0f, -7.5f);
            offset = initialOffset;
        }

        public void UpdateCameraMotion()
        {
            if (moving)
            {
                Vector3 playerPosition = playerTransform.position;
                cameraTransform.position = new Vector3(playerPosition.x, 0, Mathf.Max(cameraSetting.minZ, playerPosition.z)) + offset;

                // Increase z over time if moving.
                offset.z += cameraSetting.speedIncrementZ * Time.deltaTime;

                // Increase/decrease z when player is moving south/north.
                if (playerMovement.IsMoving)
                {
                    if (playerMovement.Direction == "north")
                    {
                        offset.z -= cameraSetting.speedOffsetZ * Time.deltaTime;
                    }
                }
            }
        }
    }


}

