using UnityEngine;
using Crossyroad;
public class PlayerMovement : MonoBehaviour
{
  
    [SerializeField]  private PlayerSetting setting;
    [SerializeField] private PlayerMoter moter;
    private IPlayerInput playerInput;
    [SerializeField] private Rigidbody rigidbody;

    private void Awake()
    {
        playerInput = new PlayerInput();
        moter = new PlayerMoter(playerInput, transform, setting, rigidbody, gameObject);
    }

    private void Update()
    {
        moter.SetCurrentPosition();
        playerInput.ReadInput(transform.position,setting);
        playerInput.Calculate(transform.position,Input.mousePosition);
        moter.Move();
        moter.MovePlayer();
    }
}