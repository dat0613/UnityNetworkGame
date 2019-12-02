using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MinNetforUnity;

public class ReadyRoomManager : MonoBehaviourMinNet
{
    ReadyRoomPanel readyRoomPanel;

	string roomName = "";
	int TeamNumber = 0;
	bool CanBargeIn = false;
	bool OnlyHeadShot = false;
	int TicketCount = 0;
	float RespawnTime = 0.0f;
	int DefaultDamage = 0;
	float HeadShotDamageMultiple = 0.0f;
	int PlayerMaxHP = 0;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        readyRoomPanel = GameObject.Find("ReadyRoomPanel").GetComponent<ReadyRoomPanel>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadRoomSetting(string roomName, int TeamNumber, bool CanBargeIn, bool OnlyHeadShot, int TicketCount, float RespawnTime, int DefaultDamage, float HeadShotDamageMultiple, int PlayerMaxHP)
    {// 일단 모든 정보를 불러오기로 함
        this.roomName = roomName;
        this.TeamNumber = TeamNumber;
        this.CanBargeIn = CanBargeIn;
        this.OnlyHeadShot = OnlyHeadShot;
        this.TicketCount = TicketCount;
        this.RespawnTime = RespawnTime;
        this.DefaultDamage = DefaultDamage;
        this.HeadShotDamageMultiple = HeadShotDamageMultiple;
        this.PlayerMaxHP = PlayerMaxHP;

        readyRoomPanel.SetRoomName(roomName);
    }
}
