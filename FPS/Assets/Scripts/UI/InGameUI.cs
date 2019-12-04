using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{

    public int maxHp = 0;
    public int nowHp = 0;

    public float maxOverheat = 0.0f;
    public float nowOverheat = 0.0f;

    [SerializeField]
    Image hpBarBackGround;

    [SerializeField]
    Image hpBarFront;

    [SerializeField]
    Image gunBackGround;

    [SerializeField]
    Image gunFront;

    [SerializeField]
    Image TicketCountBackGround;

    [SerializeField]
    TicketCounter blueTeamCounter;

    [SerializeField]
    TicketCounter redTeamCounter;

    [SerializeField]
    Text nickNameText;

    public void SetOption(int maxHp, int nowHp, float maxOverheat, float nowOverheat)
    {
        this.maxHp = maxHp;
        SetNowHp(nowHp);
    
        this.maxOverheat = maxOverheat;
    }

    public void SetNowHp(int nowHp)
    {
        this.nowHp = nowHp;

        float ratio = (float)nowHp / (float)maxHp;
        hpBarFront.fillAmount = ratio;

        hpBarFront.color = new Color(1.0f, ratio, ratio, 140.0f / 255.0f);
    }

    public void SetNowOverheat(float nowOverheat)
    {
        this.nowOverheat = nowOverheat;

        float ratio = nowOverheat / maxOverheat;
        gunFront.fillAmount = ratio;

        float col = 1.0f - ratio;
        gunFront.color = new Color(1.0f, col, col, 140.0f / 255.0f);// 총기과 과열되는 효과를 내기위해 점점 붉은 색으로 바뀜
    }

    public void SetNickName(string nikcName)
    {
        nickNameText.text = nikcName;
    }

    public void SetMaxTicket(int maxTicket)
    {
        redTeamCounter.SetMaxTicket(maxTicket);
        blueTeamCounter.SetMaxTicket(maxTicket);
    }

    public void SetNowTicket(int blueTeamNowTicket, int redTeamNowTicket)
    {
        blueTeamCounter.SetNowTicket(blueTeamNowTicket);
        redTeamCounter.SetNowTicket(redTeamNowTicket);
    }

    public void SetVisible(bool visible)
    {
        hpBarBackGround.enabled = 
        hpBarFront.enabled = 
        gunBackGround.enabled = 
        gunFront.enabled = 
        TicketCountBackGround.enabled =
        nickNameText.enabled = 
        visible;

        blueTeamCounter.SetVisible(visible);
        redTeamCounter.SetVisible(visible);
    }

}
