using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerManager
{
    static List<PlayerMove> playerList = new List<PlayerMove>();
    static Dictionary<int, PlayerMove> playerMap = new Dictionary<int, PlayerMove>();
    static PlayerMove myPlayer = null;

    public static void SetMyPlayer(PlayerMove player)
    {
        if(player.isMine)
            myPlayer = player;        
    }

    public static PlayerMove GetMyPlayer()
    {
        return myPlayer;
    }

    public static PlayerMove GetPlayer(int id)
    {
        PlayerMove player = null;
        playerMap.TryGetValue(id, out player);
        return player;
    }

    public static void AddPlayer(PlayerMove player)
    {
        PlayerMove p = null;
        
        if(playerMap.TryGetValue(player.objectId, out p))
        {
            Debug.Log(player.objectId + " 는 이미 추가된 플레이어 입니다");
            return;
        }

        SetMyPlayer(player);

        playerMap.Add(player.objectId, player);
        playerList.Add(player);
    }

    public static void DelPlayer(PlayerMove player)
    {
        if(player == null)
            return;

        PlayerMove p = null;

        if(playerMap.TryGetValue(player.objectId, out p))
        {
            playerMap.Remove(player.objectId);
            playerList.Remove(player);
        }
        else
        {

        }
    }

    public static void DelPlayer(int id)
    {
        DelPlayer(GetPlayer(id));
    }
}
