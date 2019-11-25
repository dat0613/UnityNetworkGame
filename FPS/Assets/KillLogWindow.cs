using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class KillLogWindow : MonoBehaviour
{
    [SerializeField]
    KillLog killLogPrefab;

    [Tooltip("킬로그의 최대 치")]
    public int maxLog;

    int testCount = 0;

    LinkedList<KillLog> objectList = new LinkedList<KillLog>();

    public bool ComesFromRight = true;
    public Vector2 Spacing = Vector2.zero;

    [Range(0.0f, 10.0f)]
    public float lerp = 1.0f;

    [Range(0.0f, 10.0f)]
    public float killLogDisappearTime = 1.0f;

    int sign = 0;
    
    void Awake()
    {
        sign = 1;
        if (!ComesFromRight)
            sign *= -1;
    }

    // void AddTestObject()
    // {
    //     var KillLog = Instantiate(killLogPrefab);
    //     KillLog.SetOption(testCount.ToString(), PlayerMove.Team.Blue, testCount.ToString(), PlayerMove.Team.Red, 1 == Random.Range(0, 2));
    //     testCount++;
    //     AddObject(KillLog);
    // }

    void Update()
    {
        int removeCount = 0;

        foreach (var killLog in objectList)
        {
            if(killLog.IsDie(killLogDisappearTime))
            {
                removeCount++;
            }
        }

        for(int i = 0; i < removeCount; i++)
        {
            RemoveOldestKillLog();
        }
    }

    void AddObject(KillLog obj)
    {
        obj.transform.SetParent(transform);
        obj.SetLocalScale();
        obj.rectTransform.localPosition = Vector3.zero + Vector3.right * sign * Spacing.x;
        obj.SetTargetPosition(Vector3.zero);
        obj.lerp = lerp;

        objectList.AddLast(obj);

        if(objectList.Count > maxLog)
        {
            var first = objectList.First.Value;
            first.AddTargetPosition(Vector3.right * sign * Spacing.x);
            first.destroyMode = true;

            objectList.RemoveFirst();
        }

        float height = obj.rectTransform.sizeDelta.y;
        var add = new Vector3(0.0f, height + Spacing.y, 0.0f);

        foreach (var killLog in objectList)
        {
            killLog.AddTargetPosition(-add);
        }
    }

    void RemoveOldestKillLog()
    {
        var first = objectList.First.Value;
        first.AddTargetPosition(Vector3.right * sign * Spacing.x);
        first.destroyMode = true;

        objectList.RemoveFirst();
    }

    public void AddKillLog(string killerName, PlayerMove.Team killerTeam, string victimName, PlayerMove.Team victimTeam, bool headShot)
    {
        var KillLog = Instantiate(killLogPrefab);
        KillLog.SetOption(killerName, killerTeam, victimName, victimTeam, headShot);
        AddObject(KillLog);
    }
}
