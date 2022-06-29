
using CrossyRoad;
using CrossyRoad;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class AcknowledgeHandler : RaiseEventListener
{
    private int acknowledgeCnt = 0;
    
    protected override void OnRaiseEventReceived(EventData eventData)
    {
        base.OnRaiseEventReceived(eventData);
        switch (raiseEventType)
        {
            case RaiseEventType.ACKNOWLEDGE_SCENE_LOAD:
                AcknowledgeScenePlayer();
                break;
        }
    }

    public void AcknowledgeScenePlayer()
    {
        acknowledgeCnt++;
        Debug.Log("scene cnt : ==========" + acknowledgeCnt +" ismaster " + PhotonNetwork.LocalPlayer.IsMasterClient +" length "+ PhotonNetwork.PlayerList.Length);

        if (acknowledgeCnt == PhotonNetwork.PlayerList.Length && PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Debug.Log("Start game ");
            acknowledgeCnt = 0;
            RaiseEvent(RaiseEventType.SPAWN_CAMERA, Photon.Realtime.ReceiverGroup.All);
            Destroy(this.gameObject);
            //start game
        }
    }
}


