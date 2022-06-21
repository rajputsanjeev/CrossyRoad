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
            cameraTransform.position =  offset;
            offsetPlayerMoveing = cameraTransform.position - playerTransform.position;
        }

        public void UpdateLateUpdate()
        {
            Vector3 targetPosition = playerTransform.position + offsetPlayerMoveing;
            offset.x = playerTransform.position.x;
            offset.y = targetPosition.y;

            #region comment Code
            //if (moving && playerMovement.playerDirection == "north" && Vector3.Distance(playerTransform.position, cameraTransform.position) > 50)
            //{
            //     cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, cameraSetting.speedOffsetZ * Time.deltaTime);
            //     offset.z = cameraTransform.position.z * cameraSetting.speedIncrementZ;
            //}
            //else if (!moving && Vector3.Distance(playerTransform.position, cameraTransform.position) > 50)
            //{
            //    cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, cameraSetting.speedOffsetZ * Time.deltaTime);
            //    offset.z = cameraTransform.position.z * cameraSetting.speedIncrementZ;
            //}
            //else if (Vector3.Distance(playerTransform.position, cameraTransform.position) < 50)
            //{
            //    cameraTransform.position = Vector3.Lerp(cameraTransform.position, offset, Time.deltaTime);
            //    offset.z += cameraSetting.speedOffsetZ * Time.deltaTime;
            //}
            #endregion

            if (GetDistance() > 50)
            {
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, offset, Time.deltaTime);
                offset.z += cameraSetting.speedOffsetZPlayerMove * Time.deltaTime;
            }
            else if (GetDistance() < 50)
            {
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, offset, Time.deltaTime);
                offset.z += Time.deltaTime;
            }
        }

        private float GetDistance()
        {
            return Vector3.Distance(playerTransform.position, cameraTransform.position);
        }
    }
}

