using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomClick : MonoBehaviour
{
    private void Awake()
    {
        Button btn=GetComponent<Button>();
        btn.onClick.AddListener(ChooseRoom);
    }


    public void ChooseRoom()
    {
        string ip = transform.GetChild(0).GetComponent<TMP_Text>().text;
        Room.Setip(ip);
    }
}

