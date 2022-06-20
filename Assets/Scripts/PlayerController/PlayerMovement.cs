using UnityEngine;
using Crossyroad;
using UnityEngine.SceneManagement;
using Photon.Chat;
using Photon.Pun;
using CrossyRoad;

namespace Crossyroad
{
    [RequireComponent(typeof(PhotonView))]

    public class PlayerMovement : RaiseEventListener
    {
        public delegate void OnGameOver();
        public static OnGameOver gameOverEvent;

        [SerializeField] private PhotonView view => GetComponent<PhotonView>();
        [SerializeField] private Rigidbody rigidbody => GetComponent<Rigidbody>();

        [SerializeField] private PlayerSetting setting;
        [SerializeField] private PlayerMoter moter;

        private IPlayerInput playerInput;
        public GameController gameController;
        
        public string playerDirection => moter.moveDirection;
        public bool IsMine => view.IsMine;
        public bool IsMoving => moter.IsMoving;

        private void Awake()
        {
            playerInput = new PlayerInput( setting , transform);
            moter = new PlayerMoter(playerInput, transform, setting, rigidbody, gameObject);
            gameController = ((GameController)GameController.Instance);

            if (IsMine)
            {
                gameController.cameraMovement.playerTransform = transform;
                gameController.cameraMovement.Init();
            }
        }

        private void Update()
        {
            if (view.IsMine)
            {
                Debug.Log("IsMoving " + IsMoving);
                moter.SetCurrentPosition();
                playerInput.ReadInput();
                playerInput.CalculateMousePosition();
                moter.SetTarget();
                moter.MovePlayer();
                moter.RotatePlayer();
            }
        }
    }
}

