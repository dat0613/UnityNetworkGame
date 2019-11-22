using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MinNetforUnity;

public class BattleFieldManager : MonoBehaviourMinNet
{
    public enum BattleFieldState { GameReady, GameStart, GameEnd, MAX }; // 게임 준비, 게임 시작, 게임 끝

    BattleFieldState state = BattleFieldState.MAX;

    public void ChangeState(BattleFieldState state, int time)
    {
        UiManager.Instance.SetTimer(time * 0.001f);

        this.state = state;

        switch (state)
        {
            case BattleFieldState.GameReady:
            
                break;

            case BattleFieldState.GameStart:

                break;

            case BattleFieldState.GameEnd:

                break;
        }

    }

    public void SyncState(int state, int time)
    {
        ChangeState((BattleFieldState)state, time);

    }
}
