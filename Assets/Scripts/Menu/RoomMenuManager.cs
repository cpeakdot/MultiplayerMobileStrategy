using System.Collections.Generic;
using MMS.Networking.Launch;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MMS.Menu
{
    public class RoomMenuManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Launcher launcher;
        [SerializeField] private GameObject roomListObjPrefab;
        [SerializeField] private Transform roomListTransform;
        private Dictionary<RoomInfo, GameObject> rooms = new Dictionary<RoomInfo, GameObject>();

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("Room List Updated " + roomList.Count);

            for (int i = 0; i < roomList.Count; i++)
            {
                if (roomList[i].RemovedFromList && rooms.ContainsKey(roomList[i]))
                {
                    Destroy(rooms[roomList[i]]);
                    rooms.Remove(roomList[i]);
                }
                else if (!rooms.ContainsKey(roomList[i]) && roomList[i].IsOpen)
                {
                    GameObject newRoom = Instantiate(roomListObjPrefab, Vector3.zero, Quaternion.identity, roomListTransform);
                    rooms.Add(roomList[i], newRoom);
                    newRoom.GetComponentInChildren<TMP_Text>().text = roomList[i].Name;
                    var i1 = i;
                    newRoom.GetComponent<Button>().onClick.AddListener(()=> launcher.JoinRoom(roomList[i1].Name));
                }
            }
        }
    }
}

