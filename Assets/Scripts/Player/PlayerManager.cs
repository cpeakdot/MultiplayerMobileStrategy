using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace MMS.PLAYER
{
    public class PlayerManager : MonoBehaviourPun, IPunObservable
    {
        public static GameObject LocalPlayerInstance;
        
        [SerializeField] private GameObject timberManPrefab;
        [SerializeField] private Color playerColor = Color.clear;
        [SerializeField] private GameObject woodInfoCanvasPrefab;

        private int totalGatheredWoodAmount = 0;
        private bool isWoodAmountUpdated = false;
        private TMP_Text woodAmountText;
        
        private const int TIMBER_COUNT = 3;
        
        private List<TimberManController> myTimbers = new List<TimberManController>(TIMBER_COUNT);

        private void Awake()
        {
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
            }
            
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            if (photonView.IsMine)
            {
                for (int i = 0; i < TIMBER_COUNT; i++)
                {
                    float randomX = UnityEngine.Random.Range(-3f, 3f);
                    float randomZ = UnityEngine.Random.Range(-3f, 3f);
                    GameObject timberInstance =
                        PhotonNetwork.Instantiate(
                            timberManPrefab.name, 
                            new Vector3(5f + randomX, 0f, 5f + randomZ), 
                            Quaternion.identity, 
                            0 , 
                            new object[] {photonView.ViewID});
                    myTimbers.Add(timberInstance.GetComponent<TimberManController>());
                }

                GameObject woodInfoCanvasInstance =
                    Instantiate(woodInfoCanvasPrefab, Vector3.zero, Quaternion.identity);
                woodAmountText = woodInfoCanvasInstance.GetComponentInChildren<TMP_Text>();
            }
            
            StartCoroutine(HandleColorSending());
        }

        private void Update()
        {
            if (!photonView.IsMine)
            {
                return;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (isWoodAmountUpdated)
            {
                if (stream.IsWriting)
                {
                    stream.SendNext(totalGatheredWoodAmount);
                }
            }

            if (stream.IsReading)
            {
                totalGatheredWoodAmount = (int)stream.ReceiveNext();
            }
        }

        public void AddWood(int amount)
        {
            totalGatheredWoodAmount += amount;
            if (photonView.IsMine)
            {
                woodAmountText.text = $"Total Wood : {totalGatheredWoodAmount}";
            }
        }

        public void SetPlayerColor(Color color)
        {
            playerColor = color;
        }

        private IEnumerator HandleColorSending()
        {
            while (playerColor == Color.clear)
            {
                yield return null;
            }   
            photonView.RPC("RPC_SetPlayerColor", RpcTarget.AllBuffered, playerColor.r, playerColor.g, playerColor.b);
        }

        [PunRPC]
        private void RPC_SetPlayerColor(float r, float g, float b)
        {
            Color color = new Color(r, g, b, 1);
            playerColor = color;
            for (int i = 0; i < myTimbers.Count; i++)
            {
                myTimbers[i].SetMyColor(playerColor);
            }
        }
    }
}

