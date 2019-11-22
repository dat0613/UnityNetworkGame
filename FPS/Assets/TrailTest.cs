using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailTest : MonoBehaviour
{
    public TrailRenderer trailRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        trailRenderer.enabled = true;
        transform.position += new Vector3( 0.0f, 1.0f, 0.0f ) * 0.1f * Time.deltaTime;
        trailRenderer.enabled = false;
    }
}
