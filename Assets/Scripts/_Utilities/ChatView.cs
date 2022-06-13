using ThirdPerson;
using ThirdPerson.Photon;
using Ediiie.Photon;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//TODO:Code Yet To be Optimized
public class ChatView : MonoBehaviour, IChatClientListener
{
    public List<ChatTextView> chatViewList;

    [SerializeField] private TMP_InputField message;
    [SerializeField] private Button sendBtn;
    [SerializeField] private Button clearBtn;
    [SerializeField] private Transform chatParent;
    [SerializeField] private ChatTextView prefab;
    [SerializeField] private ChatTextView selfPrefab;
    [SerializeField] private GameObject groupBtn;
    [SerializeField] private GameObject privateBtn;
    [SerializeField] private TextMeshProUGUI placeHolderText;
    [SerializeField] private RectTransform[] privateChatContents;
    [SerializeField] private ScrollRect privateChatScrollRect;
    [SerializeField] private ScrollRect publicChatScrollRect;
    [SerializeField] private TextMeshProUGUI privateHeaderText;

    Dictionary<string, int> privateChatMap;

    public bool isPrivateChat;
    public string privateChatTargetId;

    public string chatRoomName;

    protected ChatClient chatClient;
    protected internal ChatAppSettings chatAppSettings;

    private void Awake()
    {
        privateChatMap = new Dictionary<string, int>();
        PrivateChat.OnChatButtonClicked += OnPrivateChatButtonClicked;
    }

    private void OnDestroy()
    {
        PrivateChat.OnChatButtonClicked -= OnPrivateChatButtonClicked;
    }


    private void OnEnable()
    {
        ResetView();
        chatViewList = new List<ChatTextView>();

        sendBtn.onClick.AddListener(OnClickSend);
        clearBtn.onClick.AddListener(OnClearClicked);
    }

    private void ResetView()
    {
        ResetMessage();
        DeleteOldChatEntries();
    }

    private void ResetMessage()
    {
        Debug.Log("Empty");
        message.text = string.Empty;
    }

    void OnClickSend()
    {
        Debug.Log("text  " + message.text + string.IsNullOrWhiteSpace(message.text) + string.IsNullOrEmpty(message.text));
        if (string.IsNullOrWhiteSpace(message.text))
        {
            Debug.Log("text  " + message.text);
            placeHolderText.color = Color.red;
            ResetMessage();
            return;
        }

        Debug.Log("Send----Msg----");
        placeHolderText.color = Color.black;

        if (isPrivateChat)
        {
            Debug.Log("Private");
            RectTransform content = ShowChatContent(privateChatTargetId, false);

            ChatTextView chatObj = Instantiate(selfPrefab, content.transform);
            chatObj.SetData(PhotonNetwork.NickName, message.text);
            chatViewList.Add(chatObj);

            chatClient.SendPrivateMessage(privateChatTargetId, message.text);
        }
        else
        {
            this.chatClient.PublishMessage(chatRoomName, message.text);
        }
        ResetMessage();
    }

    void OnClearClicked()
    {
        Debug.Log("Clear");
        placeHolderText.color = Color.black;
        message.Select();
        ResetMessage();
    }

    void OnRecieveMsg(string sender, string msg)
    {
        ChatTextView chatPrefab = prefab;
        if (PhotonNetwork.NickName == sender)
        {
            chatPrefab = selfPrefab;
        }

        ChatTextView chatObj = Instantiate(chatPrefab, chatParent);
        chatObj.SetData(sender, msg);
        chatViewList.Add(chatObj);
    }

    void DeleteOldChatEntries()
    {
        if(chatViewList.Count > 0)
        {
            foreach (var item in chatViewList)
            {
                Destroy(item.gameObject);
            }
            
            chatViewList.Clear();
        }
   
    }

    public void Connect()
    { 
        Debug.Log("Connect--------------Chat............ " + PhotonNetwork.CurrentRoom.Name);

        chatRoomName = PhotonNetwork.CurrentRoom.Name;
        this.chatClient = new ChatClient(this);

#if !UNITY_WEBGL
        this.chatClient.UseBackgroundWorkerForSending = true;
#endif

        this.chatClient.AuthValues = new AuthenticationValues(PhotonNetwork.LocalPlayer.NickName);
        this.chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
        Debug.Log(this.chatAppSettings);
        bool isSuccess = this.chatClient.ConnectUsingSettings(this.chatAppSettings);
        Debug.Log("connect is success : " + isSuccess);
    }

    public void Update()
    {
        if (this.chatClient != null)
        {
            //Debug.Log(chatClient);
            this.chatClient.Service();
        }
    }

    public void OnApplicationQuit()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Disconnect();
        }
    }

    public void OnConnected()
    {
        Debug.Log("On chat connected");

        ChannelCreationOptions channelCreationOptions = new ChannelCreationOptions();
        channelCreationOptions.PublishSubscribers = true;


        this.chatClient.Subscribe(chatRoomName,0,100,channelCreationOptions);
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        string msgs = "";

        for (int i = 0; i < senders.Length; i++)
        {
            OnRecieveMsg(senders[i], (string)messages[i]);
        }

        Console.WriteLine("OnGetMessages: {0} ({1}) > {2}", channelName, senders.Length, msgs);
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Console.WriteLine("OnPrivateMessage: {0} ({1}) > {2}", channelName, sender, message);
        Debug.Log("Private message " + channelName + " " + sender +" "+ message);
        // All private messages are automatically cached in `ChatClient.PrivateChannels`, so you don't have to keep track of them.
        // A channel name is applied as key for `PrivateChannels`.
        // Get a (remote) user's channel name with `ChatClient.GetPrivateChannelNameByUser(name)`.
        // e.g. To get and show all messages of a private channel:
        // ChatChannel ch = this.chatClient.PrivateChannels[ channelName ];
        // foreach ( object msg in ch.Messages )
        // {
        //     Console.WriteLine( msg );
        // }

        //   privateChatTargetId = sender;
        if (!isPrivateChat)
        {
            List<PlayerLobbyView> playerLobbyViews = ((LobbyRoomController)PhotonListener<PlayerStatus>.Instance).playerLobbyViews;

            foreach (var item in playerLobbyViews)
            {
                if (sender == item.playerName)
                {
                    if (item.GetComponent<PrivateChat>() != null)
                    {
                        PrivateChat privateChat = item.GetComponent<PrivateChat>();
                        privateChat.chatNumberText.gameObject.SetActive(true);
                        privateChat.image.gameObject.SetActive(true);
                        int chatCount = privateChat.chatNumber++;
                        privateChat.chatNumberText.text = (chatCount).ToString();
                        Debug.Log(" Sender name " + sender + " Count " + privateChat.chatNumber);
                    }


                }
            }
        }
    

        RectTransform content = ShowChatContent(sender, false);
        ChatTextView chatObj = Instantiate(prefab, content.transform);
        chatObj.SetData(sender, message.ToString());
        chatViewList.Add(chatObj);
    }

    private RectTransform ShowChatContent(string playerId, bool show = true)
    {
        RectTransform content = null;

        if (!privateChatMap.ContainsKey(playerId))
        {
            content = SetAvailablePrivateContent(playerId);
        }
        else
        {
            content = privateChatContents[privateChatMap[playerId]];
        }

        if (show)
        {
            privateChatScrollRect.content = content;
            content.gameObject.SetActive(true);
        }

    

        return content;
    }

    public void OnUnsubscribed(string[] channels)
    {
        //chatRoomName = PhotonNetwork.CurrentRoom.Name;
        //this.chatClient.Subscribe(new string[] { chatRoomName });
    }

    public void OnOpponentChanged(PlayerStatus playerStatus, string photonId)
    {
        switch (playerStatus)
        {
            case PlayerStatus.OPPONENT_LEFT:
                RemovePrivateMapContent(photonId);
                break;

            case PlayerStatus.OPPONENT_JOINED:
                SetAvailablePrivateContent(photonId);
                break;
        }
    }

    public void OnLeftRoom()
    {
        Debug.Log("Unsubscribing....");
        chatClient.Unsubscribe(new string[] { chatRoomName });
        publicChatScrollRect.gameObject.SetActive(true);
        privateChatScrollRect.gameObject.SetActive(false);
        privateBtn.SetActive(false);
        isPrivateChat = false;

        for (int i = 0; i < privateChatContents.Length; i++)
        {
            ClearPrivateContent(privateChatContents[i]);
        }

        foreach (Transform item in publicChatScrollRect.content.transform)
        {
            Destroy(item.gameObject);
        }
    }

    public void OnPrivateChatButtonClicked(string reciverId , TextMeshProUGUI textMeshProUGUI , int number)
    {
        privateChatTargetId = reciverId;
        isPrivateChat = true;
        privateBtn.SetActive(true);
        privateHeaderText.text = reciverId;
        privateBtn.GetComponent<Toggle>().isOn = true;
        
        List<PlayerLobbyView> playerLobbyViews = ((LobbyRoomController)PhotonListener<PlayerStatus>.Instance).playerLobbyViews;

        foreach (var item in playerLobbyViews)
        {
            if (reciverId == item.playerName)
            {
                PrivateChat privateChat = item.GetComponent<PrivateChat>();
                privateChat.chatNumberText.text = "";
                privateChat.chatNumber = 1;
                privateChat.chatNumberText.gameObject.SetActive(false);
                privateChat.image.gameObject.SetActive(false);
               
                Debug.Log(" Sender name " + reciverId + " Count " + privateChat.chatNumber);
            }
        }
        //SendPublicMessage(false);
        HideAllContents();
        SendPublicMessage(false);
        ShowChatContent(reciverId);
    }

    private void RemovePrivateMapContent(string playerId)
    {
        if (privateHeaderText.text == privateChatTargetId)
        {
            privateHeaderText.text = "Private";
            privateChatTargetId = "";
            publicChatScrollRect.gameObject.SetActive(true);
            privateBtn.SetActive(false);
            privateChatScrollRect.gameObject.SetActive(false);
            isPrivateChat = false;
        }
        foreach (KeyValuePair<string, int> item in privateChatMap)
        {
            Debug.Log("Key " + item.Key +" Player Id " + playerId);
        }
        if (privateChatMap.ContainsKey(playerId))
        {
            ClearPrivateContent(privateChatContents[privateChatMap[playerId]]);
            privateChatMap.Remove(playerId);
        }
   
    }

    private void ClearPrivateContent(RectTransform content)
    {
        //Debug.Log("Remove " + privateChatTargetId + " " + privateHeaderText.text  +" Count "+ content.transform.childCount +" GameObjectname" + content.gameObject.name);
       
        if(content.transform.childCount > 0)
        {
            foreach (Transform item in content.transform)
            {
              //  Debug.Log("Destrpoy");
                Destroy(item.gameObject);
            }
        }
       
    }

    private void HideAllContents()
    {
        for (int i = 0; i < privateChatContents.Length; i++)
        {
            privateChatContents[i].gameObject.SetActive(false);
        }
    }

    public void SendPublicMessage(bool isOn)
    {
        isPrivateChat = !isOn;
        privateChatScrollRect.gameObject.SetActive(!isOn);
        publicChatScrollRect.gameObject.SetActive(isOn);
    }

    private RectTransform SetAvailablePrivateContent(string playerId)
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
        {
            if (!privateChatMap.ContainsValue(i))
            {
                privateChatMap.Add(playerId, i);
                Debug.Log("playerId Add  " + playerId);
                return privateChatContents[i];
            }
        }

        return null;
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {

        ChatChannel publicChannel;
        if (chatClient.TryGetChannel(this.chatRoomName, false, out publicChannel))
        {
            List<object> messages = publicChannel.Messages;
            List<string> senders = publicChannel.Senders;

            for (int i = 0; i < messages.Count; i++)
            {
                Debug.Log("Fetching grp chat " + (string)messages[i] + " Sender " + senders[i]);
                OnRecieveMsg((string)messages[i], senders[i]);
            }
        }
    }

    public void OnUserSubscribed(string channel, string user)
    {
      
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
    }

    public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void OnChatStateChange(ChatState state)
    {
    }

    public void OnDisconnected()
    {
    }
}
