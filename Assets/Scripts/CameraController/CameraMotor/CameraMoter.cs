using CrossyRoard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrossyRoad.CamerMotor
{
    public class CameraMoter
    {
        private Transform playerTransform;
        private Transform cameraTransform;
        private CameraSetting cameraSetting;
        private Vector3 offset;
        private Vector3 playerOffset;
        private Vector3 initialOffset;


        public CameraMoter(Transform playerTransform, Transform cameraTransform , CameraSetting cameraSetting  , Vector3 initialOffset)
        {
            this.playerTransform = playerTransform;
            this.cameraTransform = cameraTransform;
            this.cameraSetting = cameraSetting; 
            this.initialOffset = initialOffset; 
            offset = this.initialOffset;
        }

        public void InitPos()
        {
            cameraTransform.position =  offset;
            playerOffset = cameraTransform.position - playerTransform.position;
        }

        public void UpdateLateUpdate()
        {
            Vector3 targetPosition = playerTransform.position + playerOffset;
            offset.x = playerTransform.position.x;
            offset.y = targetPosition.y;


            if (GetDistance() > cameraSetting.distance)
            {
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, offset, Time.deltaTime);
                offset.z += cameraSetting.speedOffsetZPlayerMove * Time.deltaTime;
            }
            else if (GetDistance() < cameraSetting.distance)
            {
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, offset, Time.deltaTime);
                offset.z += Time.deltaTime;
            }

           if(GetDistance() < cameraSetting.deathZone)
            {
                Debug.Log("Death");
                MyEventArgs.UIEvents.IsMoveAble.Dispatch(false);
                MyEventArgs.UIEvents.OnPlayerDie.Dispatch(PlayerStatus.ON_PLAYER_DIE);
            }
        }

        private float GetDistance()
        {
            //Debug.Log("Diatance " + Vector3.Distance(playerTransform.position, new Vector3(cameraTransform.transform.position.x, playerTransform.position.y, cameraTransform.position.z)));
            return Vector3.Distance(playerTransform.position, new Vector3(cameraTransform.transform.position.x,playerTransform.position.y,cameraTransform.position.z));
        }

    }
}

