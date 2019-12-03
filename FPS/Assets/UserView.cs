using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class UserView : MonoBehaviour
{
    [SerializeField]
    Text nickNameText;

    [SerializeField]
    Image backGroundImage;

    [SerializeField]
    Image crownImage;

    [HideInInspector]
    public ReadyUser user = null;

    void Awake()
    {
        SetUser(null);
    }

    public void SetUser(ReadyUser user)
    {
        this.user = user;
        
        Reload();
    }

    public void Reload()
    {// 다시로딩
        Color color = backGroundImage.color;

        if(this.user == null)
        {
            if(nickNameText != null)
                nickNameText.text = "비어있음";

            color.a = 10.0f / 255.0f;
            if(crownImage != null)
                crownImage.enabled = false;
        }
        else
        {
            if(nickNameText != null)
                nickNameText.text = this.user.nickName;
            color.a = 110.0f / 255.0f;

            if(crownImage != null)
                crownImage.enabled = this.user.isMaster;
        }

        if(backGroundImage != null)
            backGroundImage.color = color;
    }
}