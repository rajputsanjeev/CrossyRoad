
namespace CrossyRoad
{
    public abstract class PhotonListener<T1> : Behaviour<View>
    {
        public static new PhotonListener<T1> Instance;
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

    }
}

