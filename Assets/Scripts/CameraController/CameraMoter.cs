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
            offset.x = cameraTransform.position.x;
            offset.y = cameraTransform.position.y;

            Debug.Log("Leteupdate");
            if (moving)
            {
                Debug.Log("  Moving  ");
                Vector3 targetPosition = playerTransform.position + offsetPlayerMoveing;
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, Time.deltaTime);
                offset.z += 0.1f;
            }
            else
            {
                Debug.Log(" Not Moving ");
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, offset, Time.deltaTime);
                offset.z += cameraSetting.speedIncrementZ * Time.deltaTime;
            }

            //shouldPos = Vector3.Lerp(cameraTransform.position, playerTransform.position,Time.deltaTime);
            //cameraTransform.position = new Vector3(shouldPos.x,1,shouldPos.z);
        }
    }
}

