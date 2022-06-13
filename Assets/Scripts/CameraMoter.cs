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
            Debug.Log("Leteupdate");

            if (moving)
            {
                Debug.Log("  Moving  ");
                Vector3 targetPosition = playerTransform.position + offsetPlayerMoveing;
                cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, targetPosition, ref velocity, SmoothTime);
            }
            //else
            //{
            //    Debug.Log(" Not Moving");
            //    if (Vector3.Distance(cameraTransform.position, playerTransform.position) < 50)
            //    {
            //        offset.z += cameraSetting.speedIncrementZ * Time.deltaTime;
            //        cameraTransform.position = Vector3.Lerp(cameraTransform.position, offset, Time.deltaTime);
            //    }
            //}
        }
    }


}

