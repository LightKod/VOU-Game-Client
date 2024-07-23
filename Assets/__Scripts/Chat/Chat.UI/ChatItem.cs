using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VOU
{
    public class ChatItem : MonoBehaviour
    {
        [SerializeField] Image userIcon;
        [SerializeField] TextMeshProUGUI txtPlayerName;
        [SerializeField] TextMeshProUGUI txtChatMsg;


        public void SetupUI(string playerName, string chatMsg)
        {
            txtPlayerName.text = playerName;
            txtChatMsg.text = chatMsg;
        }
    }
}
