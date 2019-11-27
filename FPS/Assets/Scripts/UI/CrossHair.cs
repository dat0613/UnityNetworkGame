using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    public bool visible = true;

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
        crossHairImage.enabled = visible;
    }

    public void HitFeedBack(int damage, bool isHeadShot)
    {
        if(!visible)
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
        if (!visible)
            return;

        killFeedBack.FeedBack();
        SoundManager.Instance.PlaySound("Kill");
    }

    public void SetVisible(bool visible)
    {
        this.visible = visible;

        crossHairImage.enabled = visible;
        hitFeedBack.SetVisible(visible);
        headShotFeedBack.SetVisible(visible);

        killFeedBack.SetVisible(visible);
    }
}
