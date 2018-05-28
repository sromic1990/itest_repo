using ExitGames.Client.Photon;
using IdiotTest.Scripts.GameScripts;
using Scripts.Utilities;
using Sourav.Utilities.Scripts.Attributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MultiplayerManager : Singleton<MultiplayerManager>, IPunCallbacks, IPunObservable
{
    const string GameVersion = "1.0";

    public BetAmount Bet;
    public RoomType RoomType;

    public int BetAmount_int;

    private bool disconnectOnJoinRoom;

    [SerializeField]
    private ConnectionStatus _status;
    public ConnectionStatus Status
    {
        get { return _status; }
        set
        {
            _status = value;
            ConnectionStatusChanged();
        }
    }

    public void Awake()
    {
        //PhotonNetwork.autoJoinLobby = true;
        Status = ConnectionStatus.NotConnected;
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.OnEventCall += PhotonNetwork_OnEventCall;
    }

    public void Connect()
    {
        if(PhotonNetwork.connected)
        {
            //PhotonNetwork.JoinRandomRoom();
            Debug.Log("Already connected");
            Status = ConnectionStatus.ConnectedToLobby;
        }
        else
        {
            Debug.Log("Not connected");
            Status = ConnectionStatus.Connecting;
            PhotonNetwork.ConnectUsingSettings(GameVersion);
        }
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public void JoinOnCreateRandomRoom()
    {
        //JoinOrCreateRoom();
        RoomType = RoomType.Random;
        JoinRandomRoom();
    }

    public void CreateChallengeRoom(string roomID)
    {
        RoomType = RoomType.Challenge;
        CreateRoom(true, roomID);
    }

    public void LeaveRoom(bool leaveRoomOnJoin = true)
    {
        if(PhotonNetwork.room != null)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            if(leaveRoomOnJoin)
            {
                disconnectOnJoinRoom = true;
            }
        }
    }

    public void JoinRandomRoom()
    {
        RoomType = RoomType.Random;
        JoinRoom();
    }

    public void JoinNamedRoom(string roomId)
    {
        RoomType = RoomType.Challenge;
        JoinRoom(roomId);
    }

    public void MultiplayerGameOver(int myFinalScore, bool hasOpponentLeft =  false)
    {
        PhotonNetwork.RaiseEvent((byte)OPCode.OpponentFinalScore, (object)myFinalScore, false, null);

        if(!GameManager.Instance.IsOpponentGameOver)
        {
            GameManager.Instance.WaitForOpponentToFinish();
        }
        else
        {
            GameManager.Instance.EvaluateAndShowResult(hasOpponentLeft);
        }
    }

    public void UpdateScore(int myScoreUpdate)
    {
        PhotonNetwork.RaiseEvent((byte)OPCode.ScoreUpdate, (object)myScoreUpdate, false, null);
    }

    public void SendRoom()
    {
        
    }

    public void StartANewMultiplayerGame()
    {
        int questionIndex = GameManager.Instance.GetRandomMultiplayerIndex();
        GameManager.Instance.SetMultiplayerIndex(questionIndex);
        Debug.Log("Question index = " + questionIndex);
        Debug.Log("questionIndex = "+questionIndex);
        PhotonNetwork.RaiseEvent((byte)OPCode.IndexSend, (object)questionIndex, false, null);
    }

    public void RematchRequest()
    {
        PhotonNetwork.RaiseEvent((byte)OPCode.RematchRequest, null, false, null);
    }

    public void RematchAccepted()
    {
        if(PhotonNetwork.isMasterClient)
        {
            StartANewMultiplayerGame();
        }
        else
        {
            PhotonNetwork.RaiseEvent((byte)OPCode.RematchAccepted, null, false, null);
        }
    }

    public void RematchRejected()
    {
        PhotonNetwork.RaiseEvent((byte)OPCode.RematchRejected, null, false, null);
    }

    private void StartMultiplayerGame()
    {
        //Get Name of opponent player
        //Set the requisite texts of name
        GameManager.Instance.PlayMultiplayerGame();
    }

    private void CreateRoom(bool hasRoomName = false, string roomName = "")
    {
        string BetString = Bet.ToString();
        Hashtable roomProps = new Hashtable();
        roomProps.Add("Bet", GetBetAmount(Bet));
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.PlayerTtl = 10000;
        roomOptions.EmptyRoomTtl = 0;
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = true;
        roomOptions.CustomRoomPropertiesForLobby = new string[]{"Bet"};
        roomOptions.CustomRoomProperties = roomProps;
        if(hasRoomName)
        {
            RoomType = RoomType.Challenge;
            PhotonNetwork.CreateRoom(roomName, roomOptions, null);
        }
        else
        {
            RoomType = RoomType.Random;
            PhotonNetwork.CreateRoom(null, roomOptions, null);
        }
    }

    private void JoinRoom(string roomName = "")
    {
        Debug.Log("<color=red>JoinRoom</color>");
        switch(RoomType)
        {
            case RoomType.Random:

                //string joinRoomName = string.Empty;

                //if(PhotonNetwork.GetRoomList().Length > 0)
                //{
                //    Debug.Log("<color=red>PhotonNetwork.GetRoomList().Length = "+PhotonNetwork.GetRoomList().Length+"</color>");
                //    foreach (RoomInfo ri in PhotonNetwork.GetRoomList())
                //    {
                //        if(ri.IsOpen)
                //        {
                //            if(ri.CustomProperties["Bet"].Equals(GetBetAmount(Bet)))
                //            {
                //                joinRoomName = ri.Name;
                //            }
                //            break;
                //        }
                //    }
                    
                //    if(!string.IsNullOrEmpty(joinRoomName))
                //    {
                //        JoinRoom(joinRoomName);
                //    }
                //    else
                //    {
                //        CreateRoom();
                //    }
                //}
                //else
                //{
                //    Debug.Log("CreateRoom");
                //    CreateRoom();
                //}

                //Hashtable ExpectedRoomProperties = new Hashtable();
                //ExpectedRoomProperties.Add("Bet", GetBetAmount(Bet));
                Hashtable ExpectedRoomProperties = new Hashtable { { "Bet", GetBetAmount(Bet) } };
                //ExpectedRoomProperties.Add("Bet", GetBetAmount(Bet));
                PhotonNetwork.JoinRandomRoom(ExpectedRoomProperties, 2);
                break;

            case RoomType.Challenge:
                PhotonNetwork.JoinRoom(roomName);
                break;
        }
        //Status = ConnectionStatus.Connecting;
    }

    public int GetBetAmount(BetAmount bet)
    {
        switch(bet)
        {
            case BetAmount.One:
                return 1;

            case BetAmount.Five:
                return 5;

            case BetAmount.Fifty:
                return 50;

            case BetAmount.Hundred:
                return 100;

            case BetAmount.FiveHundred:
                return 500;

            default: // BetAmount.OneThousand
                return 1000;
        }
    }

    private void ExtractInformationFromRoom()
    {
        if (!PhotonNetwork.inRoom)
            return;
        Debug.Log("InRoom");
        Hashtable roomProperties = PhotonNetwork.room.CustomProperties;
    }

    [Button()]
    public void PrintAllRooms()
    {
        Debug.Log("PrintAllRooms");
        foreach(RoomInfo ri in PhotonNetwork.GetRoomList())
        {
            Debug.Log("roomName = "+ri.Name);
        }
    }

    #region Photon Manager Methods
    private void ConnectionStatusChanged()
    {
        switch(Status)
        {
            case ConnectionStatus.ConnectedToLobby:
                UIManager.Instance.HidePopUp();
                GameDataManager.Instance.CurrentGameMode = GameMode.Multiplayer;
                if(GameManager.Instance.CurrentMultiplayerGameplay == MultiplayerGameplayType.Person)
                {
                    ScreenManager.Instance.SetANewScreen(ScreensEnum.MultiplayerMode);
                }
                //ConnectingPanel.Hide();
                //NotConnectedPanel.Hide();
                //ConnectedPanel.Show();
                //RoomPanel.gameObject.Hide();
                break;

            case ConnectionStatus.Connecting:
                //ConnectingPanel.Show();
                //NotConnectedPanel.Hide();
                //ConnectedPanel.Hide();
                //RoomPanel.gameObject.Hide();
                UIManager.Instance.ShowPopUp("Connecting To Network", null, TypeOfPopUpButtons.NoButton, TypeOfPopUp.Evented, 0, null, null);
                break;

            case ConnectionStatus.Disconnected:
                UIManager.Instance.ShowPopUp("You are disconnected from Network", null, TypeOfPopUpButtons.Ok, TypeOfPopUp.Buttoned, 0, null, null);
                GameDataManager.Instance.CurrentGameMode = GameMode.SinglePlayer;
                UIManager.Instance.HideAllQuestionPanels();
                ScreenManager.Instance.SetANewScreen(ScreensEnum.MainMenu);
                //ConnectingPanel.Hide();
                //NotConnectedPanel.Show();
                //ConnectedPanel.Hide();
                //RoomPanel.gameObject.Hide();
                break;

            case ConnectionStatus.JoinedRoom:
                //ConnectingPanel.Hide();
                //NotConnectedPanel.Hide();
                //ConnectedPanel.Hide();
                //RoomPanel.gameObject.Show();
                break;
        }
    }

    private static void PlayerLeft()
    {
        GameManager.Instance.AmIAloneInRoom = true;
        if(GameManager.Instance.IsGameStarted)
        {
            GameManager.Instance.IsOpponentGameOver = true;
            Debug.Log("From OnMasterClientSwitched");
            GameManager.Instance.CheckAndEvaluateMultiplayer(true);
        }
        else
        {
            UIManager.Instance.ShowPopUp("Please try again!", null, TypeOfPopUpButtons.Ok, TypeOfPopUp.Buttoned, 0, null, null);
            GameManager.Instance.CancelFindingmatch();
        }
    }
    #endregion

    #region IPUNCallbacks
    public void OnConnectedToPhoton()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        //throw new System.NotImplementedException();
    }

    public void OnLeftRoom()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Status = ConnectionStatus.ConnectedToLobby;
        if(GameManager.Instance.CurrentMultiplayerGameplay == MultiplayerGameplayType.AI)
        {
            GameManager.Instance.StartAIPlay();
        }
        //throw new System.NotImplementedException();
    }

    public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        PlayerLeft();
        //throw new System.NotImplementedException();
    }

    public void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Status = ConnectionStatus.ConnectedToLobby;
        //throw new System.NotImplementedException();
    }

    public void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        //JoinOrCreateRoom();
        if(RoomType == RoomType.Random)
        {
            CreateRoom();
        }
        else if(RoomType == RoomType.Challenge)
        {
            UIManager.Instance.ShowPopUp("Sorry! Room unavilable!", null, TypeOfPopUpButtons.Ok, TypeOfPopUp.Buttoned, 0, GameManager.Instance.ChallengeRoomJoinFailed, null );
        }
        //throw new System.NotImplementedException();
    }

    public void OnCreatedRoom()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Status = ConnectionStatus.JoinedRoom;
        if(RoomType == RoomType.Random)
        {
            GameManager.Instance.CreatedRoom_Random();
        }
        else if(RoomType == RoomType.Challenge)
        {
            GameManager.Instance.CreatedRoom_Challenge();
        }
    }

    public void OnJoinedLobby()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Status = ConnectionStatus.ConnectedToLobby;
        //throw new System.NotImplementedException();
    }

    public void OnLeftLobby()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        //throw new System.NotImplementedException();
    }

    public void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Status = ConnectionStatus.Disconnected;
        //throw new System.NotImplementedException();
    }

    public void OnConnectionFail(DisconnectCause cause)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Status = ConnectionStatus.Disconnected;
        //throw new System.NotImplementedException();
    }

    public void OnDisconnectedFromPhoton()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        disconnectOnJoinRoom = false;
        Status = ConnectionStatus.Disconnected;
        //throw new System.NotImplementedException();
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        throw new System.NotImplementedException();
    }

    public void OnReceivedRoomListUpdate()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        //throw new System.NotImplementedException();
    }

    public void OnJoinedRoom()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        UIManager.Instance.HidePopUp();
        if(disconnectOnJoinRoom)
        {
            disconnectOnJoinRoom = false;
            LeaveRoom();
        }
        else
        {
            Status = ConnectionStatus.JoinedRoom;
            ExtractInformationFromRoom();
            GameManager.Instance.Joined_RandomRoom();
        }
        //throw new System.NotImplementedException();
    }

    public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Debug.Log("Number of players in Room = "+PhotonNetwork.room.PlayerCount);
        if(PhotonNetwork.room.PlayerCount == 2)
        {
            GameManager.Instance.Joined_RandomRoom();
            if(PhotonNetwork.isMasterClient)
            {
                StartANewMultiplayerGame();

                //StartMultiplayerGame();
            }
        }
        //throw new System.NotImplementedException();
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        PlayerLeft();
        //throw new System.NotImplementedException();
    }

    public void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Debug.Log("code = "+codeAndMsg[0].ToString());
        Debug.Log("msg = " + codeAndMsg[1].ToString());
        CreateRoom();
        //throw new System.NotImplementedException();
    }

    public void OnConnectedToMaster()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        PhotonNetwork.JoinLobby();
        //throw new System.NotImplementedException();
    }

    public void OnPhotonMaxCccuReached()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        throw new System.NotImplementedException();
    }

    public void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        //throw new System.NotImplementedException();
    }

    public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        //throw new System.NotImplementedException();
    }

    public void OnUpdatedFriendList()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        throw new System.NotImplementedException();
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        throw new System.NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        throw new System.NotImplementedException();
    }

    public void OnWebRpcResponse(OperationResponse response)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        throw new System.NotImplementedException();
    }

    public void OnOwnershipRequest(object[] viewAndPlayer)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        throw new System.NotImplementedException();
    }

    public void OnLobbyStatisticsUpdate()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        throw new System.NotImplementedException();
    }

    public void OnPhotonPlayerActivityChanged(PhotonPlayer otherPlayer)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        //if(PhotonNetwork.room.Player)
        //throw new System.NotImplementedException();
    }

    public void OnOwnershipTransfered(object[] viewAndPlayers)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        throw new System.NotImplementedException();
    }
    #endregion

    #region IPUNObservable
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
        throw new System.NotImplementedException();
    }
    #endregion

    private void PhotonNetwork_OnEventCall(byte eventCode, object content, int senderId)
    {
        OPCode EventReceived = (OPCode)eventCode;
        Debug.Log("OpCode Received = "+EventReceived.ToString());
        switch(EventReceived)
        {
            case OPCode.GameStart:
                if(!PhotonNetwork.isMasterClient)
                {
                    StartMultiplayerGame();
                }
                break;

            case OPCode.OpponentFinalScore:
                GameManager.Instance.IsOpponentGameOver = true;
                GameManager.Instance.MultiplayerOpponentScore = (int)content;
                if(GameManager.Instance.IsMyGameOver)
                {
                    Debug.Log("From PhotonNetwork_OnEventCall OpponentFinalScore");
                    GameManager.Instance.CheckAndEvaluateMultiplayer();
                }
                break;

            case OPCode.ScoreUpdate:
                GameManager.Instance.MultiplayerOpponentScoreUpdate((int)content);
                break;

            case OPCode.IndexSend:
                if(!PhotonNetwork.isMasterClient)
                {
                    GameManager.Instance.SetMultiplayerIndex((int)content);
                    PhotonNetwork.RaiseEvent((byte)OPCode.OpponentReceivedIndex, null, false, null);
                }
                break;

            case OPCode.OpponentReceivedIndex:
                if(PhotonNetwork.isMasterClient)
                {
                    PhotonNetwork.RaiseEvent((byte)OPCode.GameStart, null, false, null);
                    StartMultiplayerGame();
                }
                break;

            case OPCode.RematchRequest:
                UIManager.Instance.ShowPopUp("Opponent has challenged for a rematch. Accept? ", null, TypeOfPopUpButtons.YesNo, TypeOfPopUp.Buttoned, 0, RematchAccepted, RematchRejected);
                break;

            case OPCode.RematchAccepted:
                if(PhotonNetwork.isMasterClient)
                {
                    StartANewMultiplayerGame();
                }
                break;

            case OPCode.RematchRejected:
                UIManager.Instance.ShowPopUp("Opponent has declined your challenge.", null, TypeOfPopUpButtons.Ok, TypeOfPopUp.Buttoned, 0, null, null);
                break;
                
        }
    }

}

public enum ConnectionStatus
{
    NotConnected,
    Connecting,
    ConnectedToLobby,
    JoinedRoom,
    Disconnected
}

public enum BetAmount
{
    One = 0,
    Five = 1,
    Fifty = 2,
    Hundred = 3,
    FiveHundred = 4,
    OneThousand = 5
}

public enum RoomType
{
    Random,
    Challenge
}

public enum OPCode : byte
{
    ScoreUpdate,
    OpponentFinalScore,
    GameStart,
    IndexSend,
    OpponentReceivedIndex,
    RematchRequest,
    RematchAccepted,
    RematchRejected
}