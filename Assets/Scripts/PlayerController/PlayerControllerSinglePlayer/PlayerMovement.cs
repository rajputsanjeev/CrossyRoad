using UnityEngine;
using CrossyRoad.PlayerMotor;
using CrossyRoad.PlayerInputSystem;
using CrossyRoard;

namespace CrossyRoad.PlayerControllerSinglePlayer
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidbody => GetComponent<Rigidbody>();

        [SerializeField] private PlayerSetting setting;
        [SerializeField] private PlayerMoter moter;

        private IPlayerInput playerInput;
        
        private string playerDirection => moter.moveDirection;
        private bool IsMoving => moter.IsMoving;
       [SerializeField] private bool IsMoveable;

        #region Unity Function
        private void Awake()
        {
            playerInput = new PlayerInput(setting , transform);
            moter = new PlayerMoter(playerInput, transform, setting, rigidbody, gameObject);
        }

        private void OnEnable()
        {
            MyEventArgs.UIEvents.IsMoveAble.AddListener(SetMoveAble);
        }

        private void OnDisable()
        {
            MyEventArgs.UIEvents.IsMoveAble.RemoveListener(SetMoveAble);
        }

        private void Update()
        {
            if (!IsMoveable)
                return;

            Debug.Log("IsMoving " + IsMoving);
            moter.SetCurrentPosition();
            playerInput.ReadInput();
            playerInput.CalculateMousePosition();
            moter.SetTarget();
            moter.MovePlayer();
            moter.RotatePlayer();
        }

        private void SetMoveAble(bool active)
        {
            rigidbody.useGravity = active;
            IsMoveable = active;
        }
        #endregion
    }
}

