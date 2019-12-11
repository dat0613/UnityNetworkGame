using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MinNetforUnity;

using UnityEngine.UI;

public class Launcher : MonoBehaviourMinNetCallBack
{
    public GameObject Object;
    [SerializeField]
    Button createButton;

    void Start()
    {
        MinNetUser.ConnectToServer("10.230.12.176", 8300);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            MinNetUser.Instantiate(Object);
        }
    }

    public override void UserEnterRoom(int roomNumber, string roomName)
    {
        // for (int i = 0; i < 10; i++)
        // {
        //     MinNetUser.Instantiate(Object);
        //     // Debug.Log(i);
        // }
        createButton.onClick.AddListener(delegate{ MinNetUser.Instantiate(Object); });

    }
}
