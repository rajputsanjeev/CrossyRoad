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
        public Vector3 offset;
        private Vector3 offsetPlayerMoveing;
        private Vector3 initialOffset;
        // change this value to get desired smoothness
        public float SmoothTime = 0.3f;

        // This value will change at the runtime depending on target movement. Initialize with zero vector.
        private Vector3 velocity = Vector3.zero;
        private Vector3 shouldPos;

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
            cameraTransform.position =  offset;
            offsetPlayerMoveing = cameraTransform.position - playerTransform.position;
        }

        public void UpdateLateUpdate()
        {
           

            Debug.Log("playerMovement.playerDirection " + playerMovement.playerDirection);
            Debug.Log("Distance " + Vector3.Distance(playerTransform.position, cameraTransform.position));
            Vector3 targetPosition = playerTransform.position + offsetPlayerMoveing;
            if (moving && playerMovement.playerDirection == "north" && Vector3.Distance(playerTransform.position, cameraTransform.position) > 50)
            {
                //Debug.Log("  Moving  ");
                 offset.x = targetPosition.x;
                 offset.y = targetPosition.y;
                 cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, Time.deltaTime);
                 offset.z = cameraTransform.position.z * cameraSetting.speedIncrementZ;
              
            }
            else
            {
                //Debug.Log(" Not Moving ");
                // Debug.Log("offset.z " + offset.z);
                offset.x = targetPosition.x;
                offset.y = targetPosition.y;
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, offset, Time.deltaTime);
                offset.z += cameraSetting.speedOffsetZ * Time.deltaTime;
            }
        }
    }
}

