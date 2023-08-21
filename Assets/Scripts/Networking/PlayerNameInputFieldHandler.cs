using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MMS.Networking.Launch
{
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerNameInputFieldHandler : MonoBehaviour
    {
        private const string playerPrefsKey = "PlayerName";

        private void Start()
        {
            string defaultName = string.Empty;
            InputField inputField = GetComponent<InputField>();
            if (inputField != null)
            {
                if(PlayerPrefs.HasKey(playerPrefsKey))
                {
                    defaultName = PlayerPrefs.GetString(playerPrefsKey);
                    inputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;
        }

        public void SetPlayerName(string playerName)
        {
            if(string.IsNullOrEmpty(playerName))
            {
                Debug.LogError("Player name is empty!");
                return;
            }

            PhotonNetwork.NickName = playerName;
            PlayerPrefs.SetString(playerPrefsKey, playerName);
        }
    }
}

