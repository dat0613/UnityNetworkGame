using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class DieInformationPanel : MonoBehaviour
{
    [SerializeField]
    Image topLabel;

    [SerializeField]
    Image bottomLabel;
    
    [SerializeField]
    Text killCountText;

    [SerializeField]
    Text killerNameText;

    [SerializeField]
    Image respawnBarBackGround;

    [SerializeField]
    Image respanwBarFront;

    [SerializeField]
    Image panel;

    float respawnTime;

    public void SetVisible(bool visible)
    {
        topLabel.enabled =
        bottomLabel.enabled = 
        killCountText.enabled = 
        killerNameText.enabled = 
        respawnBarBackGround.enabled = 
        respanwBarFront.enabled = 
        panel.enabled = visible;
    }

    public void SetOption(string killerName, int myKillCount, int killersKillCount, float respawnTime)
    {
        killerNameText.text = "<color=#ff0000>" + killerName + "</color> 에게 죽음";
        killCountText.text = "상대와의 전적 <color=#0000ff>" + myKillCount + "</color> : <color=#ff0000>" + killersKillCount + "</color>";

        StartCoroutine("FillRespawnBar");

        this.respawnTime = respawnTime;
    }

    IEnumerator FillRespawnBar()
    {
        float now = 0.0f;

        while(true)
        {
            now += Time.deltaTime;

            respanwBarFront.fillAmount = now / respawnTime;// 리스폰 게이지를 조금씩 채움

            yield return null;
        }
    }
}