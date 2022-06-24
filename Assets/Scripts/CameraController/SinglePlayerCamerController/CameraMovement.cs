using UnityEngine;
using System.Collections;
using CrossyRoad.CamerMotor;
using CrossyRoard;
using System;
using System.Collections.Generic;

namespace CrossyRoad.TileController.CameraController
{
    public class CameraMovement: MonoBehaviour
    {
        [SerializeField] public bool isMovable;

        public static CameraMovement Instance;

        [SerializeField] public Transform currentPlayerTransform;

        [SerializeField] private CameraSetting cameraSetting;
        [SerializeField] private Vector3 offset;
        [SerializeField] private List<Transform> playerTransforms = new List<Transform>();

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

        protected  void OnEnable()
        {
            MyEventArgs.UIEvents.cameraTransforms.AddListener(PlayerTransforms);
            MyEventArgs.UIEvents.PlayerMove.AddListener(ChangeCameraTargert);
            MyEventArgs.UIEvents.StartGame.AddListener(GameStart);
        }

        protected  void OnDisable()
        {
            MyEventArgs.UIEvents.cameraTransforms.RemoveListener(PlayerTransforms);
            MyEventArgs.UIEvents.PlayerMove.RemoveListener(ChangeCameraTargert);
            MyEventArgs.UIEvents.StartGame.RemoveListener(GameStart);
        }

        private void GameStart(bool end)
        {
            isMovable = end;
        }

        private void PlayerTransforms(Transform playerTransform , bool isMaster)
        {
            playerTransforms.Add(playerTransform);
            currentPlayerTransform = playerTransforms[0];
        }

        private void ChangeCameraTargert(Transform changeTransform)
        {
           if(changeTransform.position.z < changeTransform.position.z)
           {
               currentPlayerTransform = changeTransform;
           }
        }

        public void LateUpdate()
        {
            if (currentPlayerTransform == null)
                return;

            if (!isMovable)
                return;

            offset.x = currentPlayerTransform.position.x;
            offset.y = 40f;

            if (GetDistance() > cameraSetting.distance)
            {
                transform.position = Vector3.Lerp(transform.position, offset, Time.deltaTime);
                offset.z += cameraSetting.speedOffsetZPlayerMove * Time.deltaTime;
            }
            else if (GetDistance() < cameraSetting.distance)
            {
                transform.position = Vector3.Lerp(transform.position, offset, Time.deltaTime);
                offset.z += Time.deltaTime;
            }

        }

        private float GetDistance()
        {
          //  Debug.Log("Diatance " + Vector3.Distance(currentPlayerTransform.position, new Vector3(transform.transform.position.x, currentPlayerTransform.position.y, transform.position.z)));
            return Vector3.Distance(currentPlayerTransform.position, new Vector3(transform.position.x, currentPlayerTransform.position.y, transform.position.z));
        }
    }

}


