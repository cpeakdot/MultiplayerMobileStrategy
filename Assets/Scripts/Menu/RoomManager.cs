using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace MMS.Menu
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform contextTransform;
        [SerializeField] private GameObject roomPlayerPrefab;
        [SerializeField] private Dictionary<Player, GameObject> playerList = new();

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            UpdatePlayerListInfo();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (playerList.TryGetValue(otherPlayer, out var obj))
            {   
                Destroy(obj);
                playerList.Remove(otherPlayer);
            }
        }

        public void UpdatePlayerListInfo()
        {
            foreach (var player in PhotonNetwork.CurrentRoom.Players)
            {
                if (playerList.TryGetValue(player.Value, out GameObject obj))
                {
                    // Player already exist
                    
                }
                else
                {
                    // New player
                    GameObject playerIns = Instantiate(roomPlayerPrefab, Vector3.zero, Quaternion.identity, contextTransform);
                    string playerNickName;
                    if (player.Value.NickName == "")
                    {
                        playerNickName = "Player " + player.Key;
                    }
                    else
                    {
                        playerNickName = player.Value.NickName;
                    }
                    playerIns.GetComponentInChildren<TMP_Text>().text = playerNickName;
                    playerList.Add(player.Value, playerIns);
                }
            }
        }
    }
}

