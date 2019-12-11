using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    [SerializeField]
    private PoolingObject poolingObject;
    [SerializeField]
    private AudioSource source;

    public void PlaySound(AudioClip clip, Vector3 position, float maxDistance, float volume)
    {
        transform.position = position;
        source.maxDistance = maxDistance;
        source.volume = volume;
        source.clip = clip;
        source.Play();
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        yield return new WaitForSeconds(source.clip.length + 0.1f);
        poolingObject.Push();
    }
}
