using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Set
{
    public string key;
    public AudioClip clip;
};

public class SoundPlayer : MonoBehaviour
{
    public AudioSource source;
    public ObjectPool pool;
    public List<Set> clipList;

    Dictionary<string, AudioClip> clipDictionary = new Dictionary<string, AudioClip>();

    void Awake()
    {
        for(int i = 0; i < clipList.Count; i++)
        {
            Set set = clipList[i];
            
            if(set.key.Equals(""))
                continue;

            clipDictionary.Add(clipList[i].key, clipList[i].clip);
        }
    }

    public void PlaySound(string ClipName)
    {
        AudioClip clip = null;
        if(clipDictionary.TryGetValue(ClipName ,out clip))
        {
            source.PlayOneShot(clip);
        }
        else
            Debug.Log(ClipName + " 는 없습니다");
    }

    public void PlaySound(string ClipName, Vector3 position, float maxDistance, float volume)
    {
        AudioClip clip = null;
        if(clipDictionary.TryGetValue(ClipName, out clip))
        {
            pool.Pop().GetComponent<SoundObject>().PlaySound(clip, position, maxDistance, volume);
        }
        else
            Debug.Log(ClipName + " 는 없습니다");
    }

    
}