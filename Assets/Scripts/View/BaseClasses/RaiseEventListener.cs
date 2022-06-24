
using CrossyRoad;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

namespace CrossyRoad
{
    public abstract class RaiseEventListener<T1> : PhotonListener<T1>
    {
        protected RaiseEventType raiseEventSendType;
        protected object sendData;
        protected ReceiverGroup sendRaiseOptions;

        protected RaiseEventType raiseEventType;

        protected override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.NetworkingClient.EventReceived += OnRaiseEventReceived;
        }

        protected override void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= OnRaiseEventReceived;
        }

        protected void RaiseEvent(RaiseEventType raiseEventType, ReceiverGroup receiverGroup, object data = null)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions()
            {
                Receivers = receiverGroup
            };

            raiseEventSendType = raiseEventType;
            sendData = data;
            sendRaiseOptions = receiverGroup;

            PhotonNetwork.RaiseEvent((byte)raiseEventType, data, raiseEventOptions, SendOptions.SendReliable);
        }

        protected virtual void OnRaiseEventReceived(EventData eventData)
        {
            string eventCode = eventData.Code.ToString();
            raiseEventType = (RaiseEventType)Enum.Parse(typeof(RaiseEventType), eventCode);
        }
    }


    public abstract class RaiseEventListener : MonoBehaviour
    {
        protected RaiseEventType raiseEventType;

        protected void OnEnable()
        {
            PhotonNetwork.NetworkingClient.EventReceived += OnRaiseEventReceived;
        }

        protected void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= OnRaiseEventReceived;
        }

        protected void RaiseEvent(RaiseEventType raiseEventType, ReceiverGroup receiverGroup, object data = null)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions()
            {
                Receivers = receiverGroup
            };

            PhotonNetwork.RaiseEvent((byte)raiseEventType, data, raiseEventOptions, SendOptions.SendReliable);
        }

        protected virtual void OnRaiseEventReceived(EventData eventData)
        {
            raiseEventType = (RaiseEventType)eventData.Code;
        }


    }


    public abstract class RaiseEventSend : MonoBehaviour
    {
        protected RaiseEventType raiseEventType;

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
