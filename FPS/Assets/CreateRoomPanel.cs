using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using MinNetforUnity;

static class UIHelper
{
    public static int GetInt(this InputField inputField)
    {
        return int.Parse(inputField.text);
    }

    public static float GetFloat(this InputField inputField)
    {
        return float.Parse(inputField.text);
    }
}

public class CreateRoomPanel : MonoBehaviour
{  
    [SerializeField]
    InputField roomNameField;

    [SerializeField]
    Dropdown teamNumberDropDown;

    [SerializeField]
    Toggle canBargeInToggle;

    [SerializeField]
    Toggle onlyHeadShotToggle;

    [SerializeField]
    InputField ticketCountField;

    [SerializeField]
    InputField respawnTimeField;

    [SerializeField]
    InputField defaultDamageField;

    [SerializeField]
    InputField headShotDamageMultipleField;

    [SerializeField]
    InputField playerMaxHPField;

    public Notification notificationPrefab;

    // 초기화 버튼을 위해 기본 값들을 미리 저장해둠
    int originalTeamNumber;
    
    bool originalCanBargeIn;
    bool originalOnlyHeadShot;

    int originalTicketCount;
    float originalRespawnTime;
    int originalDefaultDamage;
    float originalHeadShotDamageMultiple;
    int originalPlayerMaxHP;

    void Awake()
    {
        originalTeamNumber = teamNumberDropDown.value;
        
        originalCanBargeIn = canBargeInToggle.isOn;
        originalOnlyHeadShot = onlyHeadShotToggle.isOn;

        originalTicketCount = ticketCountField.GetInt();
        originalRespawnTime = respawnTimeField.GetFloat();
        originalDefaultDamage = defaultDamageField.GetInt();
        originalHeadShotDamageMultiple = headShotDamageMultipleField.GetFloat();
        originalPlayerMaxHP = playerMaxHPField.GetInt();
    }

    public void Reset()
    {
        roomNameField.text = "";

        teamNumberDropDown.value = originalTeamNumber; 
        
        canBargeInToggle.isOn = originalCanBargeIn;
        onlyHeadShotToggle.isOn = originalOnlyHeadShot;

        ticketCountField.text = originalTicketCount.ToString();
        respawnTimeField.text = originalRespawnTime.ToString();
        defaultDamageField.text = originalDefaultDamage.ToString();
        headShotDamageMultipleField.text = originalHeadShotDamageMultiple.ToString();
        playerMaxHPField.text = originalPlayerMaxHP.ToString();
    }

    public void Cancle()
    {
        Reset();
        gameObject.SetActive(false);
    }

    public void CreateRoom()
    {
        var nowRoomName = roomNameField.text;

        var nowTeamNumber = teamNumberDropDown.value;
        
        var nowCanBargeIn = canBargeInToggle.isOn;
        var nowOnlyHeadShot = onlyHeadShotToggle.isOn;

        var nowTicketCount = ticketCountField.GetInt();
        var nowRespawnTime = respawnTimeField.GetFloat();
        var nowDefaultDamage = defaultDamageField.GetInt();
        var nowHeadShotDamageMultiple = headShotDamageMultipleField.GetFloat();
        var nowPlayerMaxHP = playerMaxHPField.GetInt();

        string errorText = "";

        if(string.IsNullOrEmpty(nowRoomName) || string.IsNullOrWhiteSpace(nowRoomName))
            errorText = "방 제목";
        else if(!(5 <= nowTicketCount && nowTicketCount <= 200))
            errorText = "팀당 최대 리스폰 수";
        else if(!(0.999f <= nowRespawnTime && nowRespawnTime <= 99.1f))
            errorText = "리스폰에 걸리는 시간";
        else if(!(1 <= nowDefaultDamage && nowDefaultDamage <= 999))
            errorText = "기본 데미지";
        else if(!(0.999f <= nowHeadShotDamageMultiple && nowHeadShotDamageMultiple <= 99.1f))
            errorText = "헤드샷 데미지 비율";
        else if(!(100 <= nowPlayerMaxHP && nowPlayerMaxHP <= 999))
            errorText = "플레이어 기본 체력";

        if(errorText != "")
        {
            var notification = Instantiate(notificationPrefab);
            notification.transform.SetParent(transform);
            notification.transform.SetAsLastSibling();
            notification.SetLocalScale();
            notification.SetText("<color=#ff6060>" +  errorText + "</color> 을(를) 다시 확인해 주세요");
        }
        else
        {
            LoadingPanel.Instance.LoadingStart(true);

            MinNetUser.CreateRoom
            (
                "ReadyRoom",
                nowRoomName,
                nowTeamNumber,
                nowCanBargeIn,
                nowOnlyHeadShot,
                nowRespawnTime,
                nowDefaultDamage,
                nowHeadShotDamageMultiple,
                nowPlayerMaxHP
            );
        }
    }
}