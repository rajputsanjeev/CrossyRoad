using CrossyRoad.Photon;
using Multiplayer;

public abstract class PhotonListener<T1> : Behaviour<View>
{
    public static PhotonListener<T1> Instance;
    protected PhotonBaseController PhotonController => PhotonBaseController.Instance;

    protected override void Awake()
    {
        base.Awake();

        if(Instance == null)
        {
            Instance = this;
        }
    }

    protected virtual void OnEnable()
    {
        Instance = this;
    }

    protected virtual void OnDisable()
    {
    }

    public abstract void OnPhotonEventExecuted(T1 data);

}

public abstract class PhotonListener<T1, T2> : Behaviour<View>
{
    public static PhotonListener<T1, T2> Instance;
    protected PhotonBaseController PhotonController => PhotonBaseController.Instance;

    protected override void Awake()
    {
        base.Awake();

        if (Instance == null)
        {
            Instance = this;
        }
    }

    protected virtual void OnEnable()
    {
        Instance = this;
    }

    protected virtual void OnDisable()
    {
    }

    public abstract void OnPhotonEventExecuted(T1 data);

    public abstract void OnPhotonEventExecuted(T1 data, T2 inform);
}
