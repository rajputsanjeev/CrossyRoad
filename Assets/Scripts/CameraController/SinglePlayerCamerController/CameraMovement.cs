using UnityEngine;
using System.Collections;
using CrossyRoad.CamerMotor;
using CrossyRoard;
using System;
using System.Collections.Generic;
using Photon.Pun;
using ExitGames.Client.Photon;

namespace CrossyRoad.Multiplayer.CameraController
{
    public class CameraMovement: PhotonListener<ExitGames.Client.Photon.Hashtable>
    {
        [SerializeField] public bool isMovable;
        [SerializeField] public Transform currentPlayerTransform;
        [SerializeField] private CameraSetting cameraSetting;
        [SerializeField] private Vector3 offset;
        [SerializeField] private List<PhotonPlayerTransform> playerTransforms = new List<PhotonPlayerTransform>();
        [SerializeField] private Vector3 playerOffset;
        [SerializeField] private Transform targetTransform;
        [SerializeField] private PhotonView photonView;
        [SerializeField] private int playerCount;

        protected override void Awake()
        {
            base.Awake();
            photonView = GetComponent<PhotonView>();

            if (photonView == null)
                return;
            transform.position = offset;
            targetTransform = GameObject.FindGameObjectWithTag("TargetPos").transform;
            playerOffset = transform.position - targetTransform.position;
            cameraSetting.distance = Mathf.Abs(playerOffset.z);
        }

        protected override void OnEnable()
        {
            MyEventArgs.UIEvents.cameraTransforms.AddListener(PlayerTransforms);
            MyEventArgs.UIEvents.PlayerMove.AddListener(ChangeCameraTargert);
            MyEventArgs.UIEvents.StartGame.AddListener(GameStart);
        }

        protected override void OnDisable()
        {
            MyEventArgs.UIEvents.cameraTransforms.RemoveListener(PlayerTransforms);
            MyEventArgs.UIEvents.PlayerMove.RemoveListener(ChangeCameraTargert);
            MyEventArgs.UIEvents.StartGame.RemoveListener(GameStart);
        }

        private void GameStart(bool end)
        {
            isMovable = end;
        }

        private void PlayerTransforms(PhotonView photonView, Transform playerTransform , bool isMaster)
        {
            PhotonPlayerTransform photonPlayerTransform = new PhotonPlayerTransform();
            photonPlayerTransform.playerTransform = playerTransform;
            photonPlayerTransform.photonView = photonView;
            playerTransforms.Add(photonPlayerTransform);
            currentPlayerTransform = playerTransforms[int.Parse(PhotonNetwork.CurrentRoom.CustomProperties["CameraPos"].ToString())].playerTransform;

            List<PhotonPlayerTransform> temp = new List<PhotonPlayerTransform>();

            playerCount++;
            if (playerCount == PhotonNetwork.PlayerList.Length)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    for (int i = 0; i < playerTransforms.Count; i++)
                    {
                        if (playerTransforms[i].photonView.IsMine)
                        {
                            temp.Add(playerTransforms[i]); 
                        }
                    }

                    for (int i = 0; i < playerTransforms.Count; i++)
                    {
                        if (!playerTransforms[i].photonView.IsMine)
                        {
                            temp.Add(playerTransforms[i]);
                        }
                    }

                }
                else
                {

                    for (int i = 0; i < playerTransforms.Count; i++)
                    {
                        if (!playerTransforms[i].photonView.IsMine)
                        {
                            temp.Add(playerTransforms[i]);
                        }
                    }

                    for (int i = 0; i < playerTransforms.Count; i++)
                    {
                        if (playerTransforms[i].photonView.IsMine)
                        {
                            temp.Add(playerTransforms[i]);
                        }
                    }
                }

                playerTransforms= temp;

                playerCount = 0;
            }
        }

        private void ChangeCameraTargert(Transform changeTransform)
        {
            int index = 0;
            Transform minTransform = playerTransforms[0].playerTransform;
            for (int i = 1; i < playerTransforms.Count; i++)
            {
                if(minTransform.position.z > playerTransforms[i].playerTransform.position.z)
                {
                    minTransform = playerTransforms[i].playerTransform;
                    index = i;
                }
            }

            Debug.Log("index " + index);
            ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
            h["CameraPos"] = index;
            PhotonNetwork.CurrentRoom.SetCustomProperties(h);
        }

        public void LateUpdate()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            if (currentPlayerTransform == null)
                return;

            if (!isMovable)
                return;

            offset.x = currentPlayerTransform.position.x;
            offset.y = 27f;

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
          //Debug.Log("cameraSetting.distance " + cameraSetting.distance + " Distance " + Vector3.Distance(currentPlayerTransform.position, new Vector3(transform.transform.position.x, currentPlayerTransform.position.y, transform.position.z)));
            return Vector3.Distance(currentPlayerTransform.position, new Vector3(transform.position.x, currentPlayerTransform.position.y, transform.position.z));
        }

        public override void OnPhotonEventExecuted(ExitGames.Client.Photon.Hashtable data)
        {
            Debug.Log(" CameraPOs " + (int)PhotonNetwork.CurrentRoom.CustomProperties["CameraPos"]);
            if (data.ContainsKey("CameraPos"))
            {
                currentPlayerTransform = playerTransforms[(int)PhotonNetwork.CurrentRoom.CustomProperties["CameraPos"]].playerTransform;
            }
        }
    }

    public class PhotonPlayerTransform 
    {
      public Transform playerTransform;
      public PhotonView photonView;
    }


}


