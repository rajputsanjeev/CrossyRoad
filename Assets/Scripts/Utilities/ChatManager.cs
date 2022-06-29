//using ExitGames.Client.Photon;
//using Photon.Chat;
//using Photon.Pun;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using BlockBabies;

////TODO:Code Yet To be Optimized
//public class ChatManager : MonoBehaviour, IChatClientListener
//{
//    public List<ChatTextView> chatViewList;

//    [SerializeField] private TMP_InputField message;
//    [SerializeField] private Button sendBtn;
//    [SerializeField] private Button clearBtn;
//    [SerializeField] private Transform chatParent;
//    [SerializeField] private ChatTextView prefab;
//    [SerializeField] private TextMeshProUGUI placeHolderText;

//    [SerializeField] private Toggle grpChat;
//    [SerializeField] private Toggle privateChat;

//    [SerializeField] private TextMeshProUGUI grpChatText;
//    [SerializeField] private TextMeshProUGUI privateChatText;

//    private string chatRoomName;

//    protected ChatClient chatClient;
//    protected internal ChatAppSettings chatAppSettings;

//    public int chatLength = 10;
//    public string targetId;
//    public static Action<string> SetPrivateChat;

//    public ScrollRect scrollRect;

//    private void Awake()
//    {
//        //#if PHOTON_UNITY_NETWORKING
//        this.chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
//        //#endif

//        Connect();
//    }

//    private void OnEnable()
//    {
//        if (this.chatClient.CanChat && PhotonNetwork.InRoom)
//        {
//            Debug.Log("Unsubscribing....");
//            chatClient.Unsubscribe(new string[] { chatRoomName });
//        }

//        ResetView();
//        chatViewList = new List<ChatTextView>();
        
//        sendBtn.onClick.AddListener(OnClickSend);
//        clearBtn.onClick.AddListener(OnClearClicked);

//        grpChat.onValueChanged.AddListener(delegate {
//            ShowGrpChatHistory();
//        });

//        grpChat.onValueChanged.AddListener(delegate {
//            ShowPrivateChat();
//        });

//        scrollRect.onValueChanged.AddListener(delegate
//        {
//            ScrollToBottom();
//        });

//        SetPrivateChat += SendPrivateMessage;
//        grpChat.isOn = true;
//    }

//    private void OnDisable()
//    {
//        sendBtn.onClick.RemoveListener(OnClickSend);
//        clearBtn.onClick.RemoveListener(OnClearClicked);

//        grpChat.onValueChanged.RemoveListener(delegate {
//            ShowGrpChatHistory();
//        });

//        grpChat.onValueChanged.RemoveListener(delegate {
//            ShowPrivateChat();
//        });

//        scrollRect.onValueChanged.RemoveListener(delegate
//        {
//            ScrollToBottom();
//        });

//        SetPrivateChat -= SendPrivateMessage;
//    }

//    public void ScrollToBottom()
//    {
//       // scrollRect.verticalNormalizedPosition = 0;
//    }

//    private void ResetView()
//    {
//        ResetMessage();
//        DeleteOldChatEntries();
//    }

//    private void ResetMessage()
//    {
//        message.text = string.Empty;
//    }

//    void ShowGrpChatHistory()
//    {
//        if (grpChat.isOn)
//        {
//           // ResetView();

//            Debug.Log("Fetching grp chat "+ chatRoomName);

//            ChatChannel publicChannel;
//            if (chatClient.TryGetChannel(this.chatRoomName, false, out publicChannel))
//            {
//                List<object> messages = publicChannel.Messages;
//                List<string> senders = publicChannel.Senders;

//                Debug.Log("Failed=========="+ messages.Count);
//                Debug.Log("Failed=========="+ senders.Count);
//                int count = messages.Count - chatViewList.Count;
//                count -= 1;
//                for (int i = chatViewList.Count; i < messages.Count; i++)
//                {
//                    Debug.Log("Fetching grp chat "+ (string)messages[i]);
//                    OnRecieveMsg((string)messages[i]);
//                    scrollRect.verticalNormalizedPosition = 0;

//                }
//            }
//            else
//            {
//                Debug.Log("Failed==========");
//            }
//        }
//        //else
//        //{
//        //    Debug.Log("Fetching private chat ");

//        //    ResetView();

//        //    ChatChannel channel;
//        //    if (chatClient.TryGetChannel(PhotonNetwork.LocalPlayer.NickName+":"+targetId,true,out channel))
//        //    {
//        //        List<object> messages = channel.Messages;
//        //        List<string> senders = channel.Senders;

//        //        for (int i = 0; i < messages.Count; i++)
//        //        {
//        //            OnRecieveMsg((string)messages[i]);
//        //        }
//        //    }
//        //}
           
//    }

//    void ShowPrivateChat()
//    {
//        if (privateChat.isOn)
//        {
//            Debug.Log("Fetching private chat ");

//            ResetView();

//            ChatChannel channel;
//            if (chatClient.TryGetChannel(PhotonNetwork.LocalPlayer.NickName + ":" + targetId, true, out channel))
//            {
//                List<object> messages = channel.Messages;
//                List<string> senders = channel.Senders;

//                for (int i = 0; i < messages.Count; i++)
//                {
//                    OnRecieveMsg((string)messages[i]);
//                }
//            }
//        }
//    }

//    void OnClickSend()
//    {
//        if (Utils.IsEmpty(message.text))
//        {
//            placeHolderText.color = Color.red;
//            ResetMessage();
//            return;
//        }

//        if (grpChat.isOn)
//        {
//            Debug.Log("Send----Msg----");
//            placeHolderText.color = Color.black;
//            this.chatClient.PublishMessage(chatRoomName, message.text);
//            ResetMessage();
//        }
//        else if(privateChat.isOn)
//        {
//            this.chatClient.SendPrivateMessage(targetId, message.text);
//            ResetMessage();
//        }

//        Debug.Log("grpChat.isOn----" + grpChat.isOn);
//        Debug.Log("privateChat.isOn----" + privateChat.isOn);

//    }

//    public void SendPrivateMessage(string reciverId)
//    {
//        targetId = reciverId; 
//        privateChat.isOn = true;
//    }

//    void OnClearClicked()
//    {
//        placeHolderText.color = Color.black;
//        message.Select();
//        ResetMessage();
//    }

//    void OnRecieveMsg(string msg)
//    {
//        Debug.Log("Recieved");
//        ChatTextView chatObj = Instantiate(prefab, chatParent);
//        chatObj.SetData("", msg);
//        chatViewList.Add(chatObj);
//    }

//    void OnRecieveMsg(string sender, string msg)
//    {
//        ChatTextView chatObj = Instantiate(prefab, chatParent);
//        chatObj.SetData(sender, msg);
//        chatViewList.Add(chatObj);
//    }

//    void DeleteOldChatEntries()
//    {
//        foreach (var item in chatViewList)
//        {
//            Destroy(item.gameObject);
//        }

//        chatViewList.Clear();
//    }

//    public void Connect()
//    {
//        Debug.Log("Connect--------------Chat............");

//        chatRoomName = PhotonNetwork.CurrentRoom.Name;
//        this.chatClient = new ChatClient(this);
  
//        #if !UNITY_WEBGL
//            this.chatClient.UseBackgroundWorkerForSending = true;
//        #endif

//        this.chatClient.AuthValues = new AuthenticationValues(PhotonNetwork.LocalPlayer.NickName);
//        this.chatClient.ConnectUsingSettings(this.chatAppSettings);
//    }

//    public void Update()
//    {
//        if (this.chatClient != null)
//        {
//            this.chatClient.Service();
//        }
//    }

//    public void OnApplicationQuit()
//    {
//        if (this.chatClient != null)
//        {
//            this.chatClient.Disconnect();
//        }
//    }

//    public void DebugReturn(DebugLevel level, string message)
//    {
//    }

//    public void OnChatStateChange(ChatState state)
//    {
//    }

//    public void OnConnected()
//    {
//        Debug.Log("On chat connected");

//        this.chatClient.Subscribe(new string[] { chatRoomName }, chatLength);
//    }

//    public void OnGetMessages(string channelName, string[] senders, object[] messages)
//    {
//        grpChat.isOn = true;

//        string msgs = "";

//        //for (int i = 0; i < senders.Length; i++)
//        //{
//        //    OnRecieveMsg(senders[i],(string)messages[i]);
//        //}
//        ShowGrpChatHistory();
//        Console.WriteLine("OnGetMessages: {0} ({1}) > {2}", channelName, senders.Length, msgs);
//    }

//    public void OnDisconnected()
//    {
//    }

//    public void OnPrivateMessage(string sender,object message,string channelName)
//    {
//        Debug.Log("channelName===" + channelName);
//        Debug.Log("sender===" + sender);
//        //privateChat.isOn = true;
//        //OnRecieveMsg(sender,(string)message);

//        string[] subs = channelName.Split(':');

//        string reciever = subs[0];
//        string senders = subs[1];

//        Debug.Log("reciver===" + reciever);
//        Debug.Log("senders===" + senders);

//        if (PhotonNetwork.LocalPlayer.NickName == sender)
//        {
//            privateChat.isOn = true;
//            privateChatText.text = senders;
//            //OnRecieveMsg(sender, (string)message);
//            Debug.Log("channelName===" + channelName);
//        }
//        else if (PhotonNetwork.LocalPlayer.NickName == reciever)
//        {
//            privateChat.isOn = true;
//            privateChatText.text = sender;
//            targetId = sender;
//            //OnRecieveMsg((string)message);
//            Debug.Log("channelName===" + channelName);
//        }

//        ShowPrivateChat();
//    }

//    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
//    {
//    }

//    public void OnSubscribed(string[] channels, bool[] results)
//    {
//    }

//    public void OnUnsubscribed(string[] channels)
//    {
//        chatRoomName = PhotonNetwork.CurrentRoom.Name;
//        this.chatClient.Subscribe(new string[] { chatRoomName }, chatLength);
//    }

//    public void OnUserSubscribed(string channel, string user)
//    {
//    }

//    public void OnUserUnsubscribed(string channel, string user)
//    {
//    }
//}
