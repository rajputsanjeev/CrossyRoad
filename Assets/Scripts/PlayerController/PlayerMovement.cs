using UnityEngine;
using Crossyroad;
using UnityEngine.SceneManagement;
using Photon.Chat;
using Photon.Pun;


namespace Crossyroad
{
    [RequireComponent(typeof(PhotonView))]

    public class PlayerMovement : MonoBehaviour, IPunObservable
    {
        public delegate void OnGameOver();
        public static OnGameOver gameOverEvent;

        [SerializeField] private PhotonView view => GetComponent<PhotonView>();
        [SerializeField] private Rigidbody rigidbody => GetComponent<Rigidbody>();

        [SerializeField] private PlayerSetting setting;
        [SerializeField] private PlayerMoter moter;

        private IPlayerInput playerInput;
        public GameController gameController;
        private Vector3 correctPlayerPos;
        private Quaternion correctPlayerRot;

        public string playerDirection => moter.MoveDirection;

        public bool IsMine => view.IsMine;
        public bool IsMoving => moter.IsMoving;

        private void OnEnable()
        {
            gameOverEvent += GameOver;
        }

        private void OnDisable()
        {
            gameOverEvent -= GameOver;
        }

        private void Awake()
        {
            playerInput = new PlayerInput(setting.xJumpDistance);
            moter = new PlayerMoter(playerInput, transform, setting, rigidbody, gameObject);
            gameController = GameController.instance;
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
                playerInput.ReadInput(transform.position, setting);
                playerInput.Calculate(transform.position, Input.mousePosition);
                moter.Move();
                moter.MovePlayer();
            }
        }

        private void GameOver()
        {
            //moter.IsMoving = false;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                //We own this player: send the others our data
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else
            {
                //Network player, receive data
                correctPlayerPos = (Vector3)stream.ReceiveNext();
                correctPlayerRot = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}

