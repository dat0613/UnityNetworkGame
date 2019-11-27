using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    Vector3 direction;
    Vector3 endPoint;
    float speed = 0.0f;
    float flightTime = 0.0f;
    Vector3 startPosition;
    float endDistance = 0.0f;

    public void SetOption(Vector3 endPoint, float speed, bool local)
    {
        var dif = endPoint - transform.position;
        this.speed = speed;
        this.endPoint = endPoint;
        direction = dif.normalized;

        endDistance = (endPoint - transform.position).sqrMagnitude;

        if(local)
        {
            transform.position -= direction * speed * Time.deltaTime;   
        }
    }

    void Start()
    {
        startPosition = transform.position;
    }
    

    void Update()
    {
        flightTime += Time.deltaTime;
        transform.position += direction * speed * Time.deltaTime;

        if (endDistance < (startPosition - transform.position).sqrMagnitude)
        {
            Destroy(gameObject);
        }
    }

}
