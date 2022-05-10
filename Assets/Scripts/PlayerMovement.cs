using UnityEngine;
using Crossyroad;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
  
    [SerializeField]  private PlayerSetting setting;
    [SerializeField] private PlayerMoter moter;
    private IPlayerInput playerInput;
    [SerializeField] private Rigidbody rigidbody;
    public bool IsMoving => moter.IsMoving;
    public string Direction => moter.MoveDirection;
    public delegate void OnGameOver();
    public static OnGameOver gameOverEvent;

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
    }

    private void Update()
    {
        moter.SetCurrentPosition();
        playerInput.ReadInput(transform.position,setting);
        playerInput.Calculate(transform.position,Input.mousePosition);
        moter.Move();
        moter.MovePlayer();
    }

    private void GameOver()
    {
        moter.IsMoving = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}