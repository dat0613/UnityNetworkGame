using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    static InputManager instance = null;
    public static InputManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public enum InputFocus { CharacterControl, Chatting, Select };

    public void ChangeFocus(InputFocus focus)
    {
        // Debug.Log("상태 변경 : " + this.focus.ToString() + " >> " + focus.ToString());

        this.focus = focus;

        switch (focus)
        {
            case InputFocus.CharacterControl:
                FollowCamera.Instance.SetMouseLock(true);
                break;

            case InputFocus.Chatting:
                FollowCamera.Instance.SetMouseLock(false);
                break;

            case InputFocus.Select:

                break;
        }

    }
    
    InputFocus focus = InputFocus.CharacterControl;

    public InputFocus Focus
    {
        get
        {
            return focus;
        }
    }

    void OnDestroy()
    {
        ChangeFocus(InputFocus.Chatting);
    }
}
