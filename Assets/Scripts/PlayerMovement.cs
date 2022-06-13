using UnityEngine;
using Crossyroad;
using UnityEngine.SceneManagement;
using Photon.Chat;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]

public class PlayerMovement : MonoBehaviour , IPunObservable
{
    [SerializeField] private PhotonView view => GetComponent<PhotonView>();

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
        if (!view.IsMine)
            return;

        Debug.Log("IsMoving " + IsMoving);
        moter.SetCurrentPosition();
        playerInput.ReadInput(transform.position,setting);
        playerInput.Calculate(transform.position,Input.mousePosition);
        moter.Move();
        moter.MovePlayer();
    }

    private void GameOver()
    {
        //moter.IsMoving = false;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}