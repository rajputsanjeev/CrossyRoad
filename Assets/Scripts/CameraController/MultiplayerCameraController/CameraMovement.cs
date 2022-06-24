using UnityEngine;
using System.Collections;
using CrossyRoad.CamerMotor;
using CrossyRoard;
using System;

namespace CrossyRoad.PlayerControllerSinglePlayer.CameraController
{
    public class CameraMovement: MonoBehaviour
    {
        public static CameraMovement Instance;
        protected CameraMoter cameraMoter;
        public Transform playerTransform;
        public CameraSetting cameraSetting;
        public Vector3 initialOffset;
        [SerializeField]
        private bool IsMoveable;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;

            Init();
        }

        private void OnEnable()
        {
            MyEventArgs.UIEvents.IsMoveAble.AddListener(SetMoveAble);
        }


        private void OnDisable()
        {
            MyEventArgs.UIEvents.IsMoveAble.RemoveListener(SetMoveAble);

        }
        private void SetMoveAble(bool obj)
        {
            IsMoveable = false;
        }

        public void Init()
        {
            cameraMoter = new CameraMoter(playerTransform, transform, cameraSetting, initialOffset);
            cameraMoter.InitPos();
        }

        public void LateUpdate()
        {
            if(playerTransform != null && IsMoveable)
               cameraMoter.UpdateLateUpdate();
        }
    }

}


