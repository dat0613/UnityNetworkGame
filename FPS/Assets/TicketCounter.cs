using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TicketCounter : MonoBehaviour
{
    [SerializeField]
    Image ticketCountImage;

    [SerializeField]
    Text ticketCountText;
    
    int nowTicket = 0;
    int maxTicket = 0;

    float targetRatio = 0.0f;
    float nowRatio = 0.0f;

    void Update()
    {
        nowRatio = Mathf.Lerp(nowRatio, targetRatio, Time.deltaTime * 6.0f);

        if(Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log(maxTicket + ", " + nowTicket + ", " + nowRatio + ", " + targetRatio);
        }  
        ticketCountImage.fillAmount = nowRatio;
    }

    public void SetMaxTicket(int maxTicket)
    {
        this.maxTicket = maxTicket;
    }

    public void SetNowTicket(int nowTicket)
    {
        this.nowTicket = nowTicket;
        targetRatio = (float)nowTicket / (float)maxTicket;
        ticketCountText.text = nowTicket.ToString();
    }

    public void SetVisible(bool visible)
    {
        ticketCountImage.enabled = 
        ticketCountText.enabled = 
        visible;
    }
}