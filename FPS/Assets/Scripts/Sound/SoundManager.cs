using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;
    private float lastVolume = 0.0f;
    public static SoundManager Instance
    {
        get
        {
            return instance;
        }
    }

    public SoundPlayer soundPlayer = null;

    void OnApplicationFocus(bool focusStatus)
    {
        if(focusStatus)
        {
            AudioListener.volume = lastVolume;
        }
        else
        {
            lastVolume = AudioListener.volume;
            AudioListener.volume = 0.0f;
        }
    }

    void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);

        lastVolume = AudioListener.volume;
    }

    public void PlaySound(string name)
    {
        soundPlayer?.PlaySound(name);
    }
    
    public void PlaySound(string name, Vector3 position)
    {
        soundPlayer?.PlaySound(name, position);
    }
}