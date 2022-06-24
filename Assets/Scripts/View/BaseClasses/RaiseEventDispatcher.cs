
using CrossyRoad;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace CrossyRoad
{
    public abstract class RaiseEventDispatcher<T1, T2> : Behaviour<T1, T2> where T1 : View
    {
        protected virtual void OnEnable()
        {
            Instance = this;
        }

        protected void RaiseEvent(RaiseEventType raiseEventType, ReceiverGroup receiverGroup, object data = null)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions()
            {
                Receivers = receiverGroup
            };

            PhotonNetwork.RaiseEvent((byte)raiseEventType, data, raiseEventOptions, SendOptions.SendReliable);
        }
    }

}
