﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MinNetforUnity;

public class BattleFieldManager : MonoBehaviourMinNet
{
    public enum BattleFieldState { GameReady, GameStart, GameEnd, MAX }; // 게임 준비, 게임 시작, 게임 끝

    BattleFieldState state = BattleFieldState.MAX;
    Canvas canvas;

    public ResultWindow resultWindowPrefab;

    void Awake()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    public void SetMaxTicket(int maxTicket)
    {
        UIManager.Instance.SetMaxTicket(maxTicket);
    }

    public void SetNowTicket(int blueTeamTicketCount, int redTeamTicketCount)
    {
        UIManager.Instance.SetNowTicket(blueTeamTicketCount, redTeamTicketCount);
    }

    public void GameEnd(int winnerTeamNumber)
    {
        var winner = (PlayerMove.Team)winnerTeamNumber;
        var window = Instantiate(resultWindowPrefab, canvas.transform);
        window.transform.SetAsLastSibling();
        window.SetOption(PlayerManager.GetMyPlayer().team == winner);
    }
}
