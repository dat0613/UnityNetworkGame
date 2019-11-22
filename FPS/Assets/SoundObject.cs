using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    [SerializeField]
    private PoolingObject poolingObject;
    [SerializeField]
    private AudioSource source;

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        transform.position = position;
        source.PlayOneShot(clip);

        StartCoroutine("CheckSoundEnd");
    }

    IEnumerator CheckSoundEnd()
    {
        while(true)
        {
            if(!source.isPlaying)
                poolingObject.Push();

            yield return 0;
        }
    }
}
