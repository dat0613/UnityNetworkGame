using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MinNetforUnity;

public class TeamSelect : MonoBehaviour
{
    public void SelectBlueTeam()
    {
        SelectTeam(PlayerMove.Team.Blue);
    }

    public void SelectRedTeam()
    {
        SelectTeam(PlayerMove.Team.Red);
    }

    private void SelectTeam(PlayerMove.Team team)
    {
        var player = PlayerManager.GetMyPlayer();
        player?.RPC("SelectTeam", MinNetRpcTarget.Server, (int)team);
    }

    void Update()
    {
        if(InputManager.Instance.Focus == InputManager.InputFocus.CharacterControl)
        {
            if(Input.GetKeyDown(KeyCode.N))
            {
                SelectBlueTeam();
            }
            
            if (Input.GetKeyDown(KeyCode.M))
            {
                SelectRedTeam();
            }
        }
    }
}