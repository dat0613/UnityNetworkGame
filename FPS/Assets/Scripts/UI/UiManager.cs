﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameTimer timer;

    [SerializeField]
    CrossHair crossHair;

    [SerializeField]
    LogWindow logWindow;

    [SerializeField]
    KillLogWindow killLogWindow;

    bool onGame = false;

    [HideInInspector]
    public bool skip1Frame = false;// 엔터를 한번 더 쳐서 채팅을 끝냈을때 엔터 이벤트가 한 프레임 동안 남아 있어서 한 프레임 스킵 해야함

    [SerializeField]
    HitCircle hitCirclePrefab;

    [SerializeField]
    Transform hitCircles;

    [SerializeField]
    HitEffect hitEffect;

    [SerializeField]
    InGameUI inGameUI;

    [SerializeField]
    DieInformationPanel dieInformationPanel;

    bool visible = true;

    static UIManager instance = null;
    public static UIManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        SetVisible(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !skip1Frame)
        {
            switch (InputManager.Instance.Focus)
            {
                case InputManager.InputFocus.CharacterControl:// 게임 도중 엔터를 눌렀다면 채팅을 시작함
                    ChattingMode();
                    InputManager.Instance.ChangeFocus(InputManager.InputFocus.Chatting);
                    break;

                case InputManager.InputFocus.Chatting:// 채팅을 끝내고 게임으로 돌아감
                    //ChattingMode();
                    //InputManager.Instance.ChangeFocus(InputManager.InputFocus.CharacterControl);
                    break;

                case InputManager.InputFocus.Select:


                    break;
            }
        }
        skip1Frame = false;
    }

    public void HitFeedBack(int damage, bool isHeadShot)
    {
        crossHair.HitFeedBack(damage, isHeadShot);
    }

    public void SetNickName(string nickName)
    {
        inGameUI.SetNickName(nickName);
    }

    public void KillFeedBack(string victimNickName)
    {
        crossHair.KillFeedBack(victimNickName);
    }

    public void SetCrossHairVisible(bool Visible)
    {
        crossHair.SetVisible(Visible);
    }

    public void SetTimer(float second)
    {
        timer.SetTimer(second);
    }

    public void ChattingMode()
    {
        logWindow.ChattingMode();
    }

    public void AddChat(string chat, Color color)
    {
        logWindow.AddLog(chat, color);
    }

    public void AddKillLog(int killerId, int victimId, bool headShot)
    {
        var killer = PlayerManager.GetPlayer(killerId);
        var victim = PlayerManager.GetPlayer(victimId);

        if (killer == null || victim == null)
            return;

        killLogWindow.AddKillLog(killer.playerName, killer.team, victim.playerName, victim.team, headShot);
    }

    public void AddHitCircle(Vector3 hitPosition)
    {
        var hitCircle = Instantiate(hitCirclePrefab);
        hitCircle.transform.SetParent(hitCircles);
        hitCircle.SetLocalScale();
        hitCircle.SetVisible(visible);

        hitCircle.SetOption(hitPosition);
    }

    public void ViewEffect(int damage)
    {
        hitEffect.ViewEffect(damage);
    }

    public void SetGameUI(int maxHp, int nowHp, float maxOverheat, float nowOverheat)
    {
        inGameUI.SetOption(maxHp, nowHp, maxOverheat, nowOverheat);
    }

    public void UpdateGameUI(int nowHp, float nowOverheat)
    {
        inGameUI.SetNowHp(nowHp);
        inGameUI.SetNowOverheat(nowOverheat);
    }

    public void SetMaxTicket(int maxTicket)
    {
        inGameUI.SetMaxTicket(maxTicket);
    }

    public void SetNowTicket(int blueTeamNowTicket, int redTeamNowTicket)
    {
        inGameUI.SetNowTicket(blueTeamNowTicket, redTeamNowTicket);
    }

    public void SetDieUI(string killerName, int myKillCount, int killersKillCount, float respawnTime)
    {
        dieInformationPanel.SetOption(killerName, myKillCount, killersKillCount, respawnTime);
    }

    public void ViewDieUI()
    {
        inGameUI.SetVisible(false);
        logWindow.SetVisible(false);
        dieInformationPanel.SetVisible(true);
        crossHair.SetVisible(false);
        killLogWindow.SetVisible(true);
        hitEffect.SetVisible(true);
    }

    public void ViewGameUI()
    {
        inGameUI.SetVisible(true);
        logWindow.SetVisible(true);
        dieInformationPanel.SetVisible(false);
        crossHair.SetVisible(true);
        killLogWindow.SetVisible(true);
        hitEffect.SetVisible(true);
    }

    public void SetVisible(bool visible)
    {
        this.visible = visible;

        crossHair.SetVisible(visible);
        logWindow.SetVisible(visible);
        killLogWindow.SetVisible(visible);
        hitEffect.SetVisible(visible);
        inGameUI.SetVisible(visible);
        dieInformationPanel.SetVisible(visible);
    }
}