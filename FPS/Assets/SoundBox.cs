using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("asdfasdf"); 
    }

    IEnumerator asdfasdf()
    {
        while(true)
        {
            SoundManager.Instance.PlaySound("Shot", transform.position, 100, 1);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        }
    }
}
