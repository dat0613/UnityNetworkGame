using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    public bool Visible = true;

    [SerializeField]
    private Image crossHairImage;
    [SerializeField]
    private CrossHairFeedBack headShotFeedBack;
    [SerializeField]    
    private CrossHairFeedBack hitFeedBack;
    [SerializeField]
    private KillFeedBack killFeedBack;
    
    void Awake()
    {
        crossHairImage.enabled = Visible;
    }

    public void HitFeedBack(int damage, bool isHeadShot)
    {
        if(!Visible)
            return;

        if(isHeadShot)
        {
            headShotFeedBack.HitFeedBack(damage);
            SoundManager.Instance.PlaySound("HeadShot");
        }
        else
        {
            hitFeedBack.HitFeedBack(damage);
            SoundManager.Instance.PlaySound("Hit");
        }
    }

    public void KillFeedBack()
    {
        if (!Visible)
            return;

        killFeedBack.FeedBack();
        SoundManager.Instance.PlaySound("Kill");
    }

    public void SetVisible(bool Visible)
    {
        this.Visible = Visible;
    }
}
