using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaTest : MonoBehaviour
{
    [SerializeField]
    MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer.material.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
