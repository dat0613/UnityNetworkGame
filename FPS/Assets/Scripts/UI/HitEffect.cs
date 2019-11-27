using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HitEffect : MonoBehaviour
{
    [SerializeField]
    Image image;

    float alpha = 0.0f;

    public float lerp;
    public float multiple = 0.01f;

    void Update()
    {
        alpha = Mathf.Lerp(alpha, 0.0f, lerp * Time.deltaTime);

        if(alpha <= 0.01f)
            alpha = 0.0f;

        image.color = new Color(1.0f, 0.0f, 0.0f, alpha);

        if(Input.GetKeyDown(KeyCode.T))
        {
            ViewEffect(20);
        }
        if(Input.GetKeyDown(KeyCode.Y))
        {
            ViewEffect(40);
        }
    }

    public void ViewEffect(int damage)
    {
        alpha += damage * multiple;
    }
}
