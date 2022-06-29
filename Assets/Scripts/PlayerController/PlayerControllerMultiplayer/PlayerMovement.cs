using UnityEngine;
using Photon.Pun;
using System;
using CrossyRoad.TileController.GameManager;
using CrossyRoad.PlayerMotor;
using CrossyRoad.PlayerInputSystem;
using Photon.Realtime;
using CrossyRoad.PlayerInstanceNamespace;
using CrossyRoard;
using TMPro;

namespace CrossyRoad.PhotonPlayerMovement
{
    [RequireComponent(typeof(PhotonView))]

    public class PlayerMovement : RaiseEventSend
    {
        public TextMeshProUGUI name;

        public bool isMovable;

        public static event Action<PlayerStatus> OnDie;

        [SerializeField] public PhotonView view { get; private set; }

        public PhotonView GetView()
        {
            return view;
        }

        [SerializeField] private Rigidbody rigidbody;

        [SerializeField] private PlayerSetting setting;
        [SerializeField] private PlayerMoter moter;

        private IPlayerInput IplayerInput;
        private IGameManager IgameManager;
       
        public string playerDirection => moter.moveDirection;

        public bool IsMine => view.IsMine;
        public bool IsMoving => moter.IsMoving;

        private PlayerInstance playerInstance;

        #region Unity Function
        private void Awake()
        {
            view = GetComponent<PhotonView>();
            rigidbody = GetComponent<Rigidbody>();

            //Transfer my position to camera
            MyEventArgs.UIEvents.cameraTransforms.Dispatch(transform, PhotonNetwork.IsMasterClient);

            IgameManager = Singleton<GameController>.Instance;
            IplayerInput = new PlayerInput(setting , transform);
            moter = new PlayerMoter(IplayerInput, transform, setting, rigidbody, gameObject);
            playerInstance = Singleton<GameController>.Instance.GetPlayerInstance(view.Owner.ActorNumber);
            name.text = playerInstance.playerName;
            RaiseEvent(RaiseEventType.PLAYER_SPAWNED, ReceiverGroup.All);
        }

        protected void OnEnable()
        {
            MyEventArgs.UIEvents.StartGame.AddListener(TimerEnd);
        }

        protected void OnDisable()
        {
            MyEventArgs.UIEvents.StartGame.RemoveListener(TimerEnd);

        }

        private void TimerEnd(bool end)
        {
            isMovable = end;
        }

        private void Update()
        {
             if (!view.IsMine)
                return;

            if (!isMovable)
                return;

             //Debug.Log("IsMoving " + IsMoving);
              moter.SetCurrentPosition();
              IplayerInput.ReadInput();
              IplayerInput.CalculateMousePosition();
              moter.SetTarget();
              moter.MovePlayer();
              moter.RotatePlayer();

            if (IsMoving)
            {
                RaiseEvent(RaiseEventType.PLAYER_MOVE, ReceiverGroup.MasterClient);
                MyEventArgs.UIEvents.PlayerMove.Dispatch(transform);
            }
        }

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            // When collide with player, flatten it!
            if (other.gameObject.tag == "Platform")
            {
                AddScore();
            }
        }

        public void AddScore()
        {
            view.RPC("AddScore", RpcTarget.MasterClient, playerInstance.playerID);
        }

        [PunRPC]
        private void AddScore(int id)
        {
            Debug.Log("id " + id);
            if(Singleton<GameController>.Instance.GetPlayerInstance(id) != null)
                Singleton<GameController>.Instance.GetPlayerInstance(id).AddScore();
        }
    }
}

