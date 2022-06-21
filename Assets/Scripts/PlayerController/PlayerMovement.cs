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
        
        public string playerDirection => moter.moveDirection;
        public bool IsMine => view.IsMine;
        public bool IsMoving => moter.IsMoving;

        private PlayerInstance playerInstance;

     

        #region Unity Function
        private void Awake()
        {
            playerInput = new PlayerInput(setting , transform);
            moter = new PlayerMoter(playerInput, transform, setting, rigidbody, gameObject);
            playerInstance = ((GameController)GameController.Instance).GetPlayerInstance(view.ViewID);
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
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            // When collide with player, flatten it!
            if (other.gameObject.tag == "Platform")
            {
                view.RPC("AddScore", RpcTarget.MasterClient , view.ViewID);
            }
        }


        [PunRPC]
        private void AddScore(int id)
        {
            ((GameController)GameController.Instance).GetPlayerInstance(id).AddScore();
        }
    }
}

