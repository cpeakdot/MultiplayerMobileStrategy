using MMS.PLAYER;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace MMS.Networking.Game
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public GameObject playerPrefab;
        
        #region Photon Callbacks
        
        public override void OnLeftRoom()
        {
            Debug.Log("Player left the room.");
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.LogFormat("Player entered room {0}", newPlayer.NickName);

            if(PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("Player Entered Room Master Client {0}", PhotonNetwork.IsMasterClient);

                LoadLevel();
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.LogFormat("Player Left Room {0}", otherPlayer.NickName);

            if(PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("Player Left Room Master Client {0}", PhotonNetwork.IsMasterClient);

                LoadLevel();
            }
        }

        #endregion

        #region Public Methods
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region Private Methods

        private void Start()
        {
            if (PlayerManager.LocalPlayerInstance == null)
            {
                GameObject player = PhotonNetwork.Instantiate(this.playerPrefab.name, Vector3.zero, Quaternion.identity, 0);

                Color randomColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f),
                    UnityEngine.Random.Range(0f, 1f), 1f);
                player.GetComponent<PlayerManager>().SetPlayerColor(randomColor);
            }
        }

        private void LoadLevel()
        {
            if(!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("Trying to load level - Not Master");
                return;
            }
            Debug.LogFormat("Photon Network : Loading Level {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Room01");
        }

        #endregion
    }
}

