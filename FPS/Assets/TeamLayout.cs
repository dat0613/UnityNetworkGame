using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamLayout : MonoBehaviour
{
    public List<UserView> userList;

    public void DelUser(ReadyUser user)
    {
        int count = userList.Count;

        for(int i = 0; i < count; i++)
        {
            if(userList[i].user == user)
            {
                userList[i].SetUser(null);
            
                for(int j = i; j < count - 1; j++)
                {
                    userList[j].SetUser(userList[j + 1].user);
                }

                break;
            }
        }
    }

    public void AddUser(ReadyUser user)
    {
        int count = userList.Count;

        for(int i = 0; i < count; i++)
        {
            if(userList[i].user == null)
            {
                userList[i].SetUser(user);
                break;
            }
        }
    }

    public void Reload()
    {
        int count = userList.Count;

        for(int i = 0; i < count; i++)
        {
            userList[i].Reload();
        }
    }
}