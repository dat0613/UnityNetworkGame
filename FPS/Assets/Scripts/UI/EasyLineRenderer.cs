using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyLineRenderer : MonoBehaviour
{
    [SerializeField]
    LineRenderer line;

    [Tooltip("바라볼 각도")]
    [Range(0.0f, 360.0f)]
    public float Angle = 0.0f;

    [Tooltip("부채꼴의 중심각")]
    [Range(0.0f, 360.0f)]
    public float CenterAngle = 45.0f;

    [Range(0.0f, 360.0f)]
    public int VertexCount = 10;

    public float Distance = 200.0f;

    void Update()
    {
        line.positionCount = VertexCount;

        float anglePerVertex = CenterAngle / VertexCount;// 한 정점 당 각도
        float nowAngle = Angle - CenterAngle * 0.5f;

        for(int i = 0; i < VertexCount; i++)
        {
            float rad = nowAngle * Mathf.Deg2Rad;
            line.SetPosition(i, new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0.0f) * Distance);
            nowAngle += anglePerVertex;
        }
    }
}
