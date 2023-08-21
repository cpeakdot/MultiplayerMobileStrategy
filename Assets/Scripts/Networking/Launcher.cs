using MMS.Menu;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace MMS.Networking.Launch
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        [SerializeField] private int maxPlayersPerRoom = 2;

        [Header("Display")]
        [SerializeField] private GameObject controlPanel;
        [SerializeField] private GameObject progressLabel;

        [Header("Room Creation")]
        [SerializeField] private RoomManager roomManager;
        [SerializeField] private TMP_InputField roomNameInputField;
        [SerializeField] private TMP_Text roomNameText;

        [SerializeField] private MenuManager menuManager;

        private bool isConnecting = false;
        private const string gameVersion = "0.0.1";

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            menuManager.OpenLoadingMenu();
            
            
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }

        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
                Debug.Log("Join Random Room");
            }
            else
            {
                isConnecting = PhotonNetwork.ConnectUsingSettings();

                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        public void CreateRoom()
        {
            if (roomNameInputField.text.Length == 0)
            {
                return;
            }
            PhotonNetwork.CreateRoom(roomNameInputField.text);
            menuManager.OpenLoadingMenu();
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            menuManager.OpenMainMenu();
        }

        public void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        public void StartGame()
        {
            PhotonNetwork.LoadLevel(1);
        }

        #region Photon Callbacks

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            menuManager.OpenMainMenu();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            menuManager.OpenMainMenu();
            Debug.Log($"On Disconnect Called with reason : {cause}");
        }

        public override void OnCreatedRoom()
        {
            Debug.Log("Room created");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions{ MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            menuManager.OpenFindRoomMenu();
            Debug.Log("Cant join room : " + message);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined to a room");
            menuManager.OpenRoomMenu();
            roomNameText.text = PhotonNetwork.CurrentRoom.Name;
            roomManager.UpdatePlayerListInfo();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.LogError(message);
        }

        #endregion
    }

}
