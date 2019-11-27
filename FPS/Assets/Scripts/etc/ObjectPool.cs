using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public PoolingObject objectPrefab;
    [Tooltip("Awake에서 자동으로 오브젝트 풀을 채울지")]
    public bool autoFill;
    public int poolSize;

    private Queue<PoolingObject> Pool = new Queue<PoolingObject>();
    
    public delegate void CallBack(PoolingObject obj);

    public CallBack OnInstantiate = null;
    public CallBack OnPop = null;
    public CallBack OnPush = null;

    public void SetOnInstantiate(CallBack callBack)
    {
        OnInstantiate = callBack;
    }

    public void SetOnPop(CallBack callBack)
    {
        OnPop = callBack;
    }

    public void SetOnPush(CallBack callBack)
    {
        OnPush = callBack;
    }

    void Awake()
    {
        if(autoFill)
            AutoFill();
    }

    public void AutoFill()
    {
        FillPool(poolSize);
    }

    public void FillPool(int size)
    {
        for(int i = 0; i < size; i++)
        {
            Push(CreateObject());
        }
    }

    private PoolingObject CreateObject()
    {
        if(objectPrefab == null)
        {
            Debug.Log("ObjectPrefeb이 null 입니다.");
            return null;
        }

        PoolingObject obj = Instantiate(objectPrefab);
        obj.parent = this;
        OnInstantiate?.Invoke(obj);
        return obj;
    }

    public PoolingObject Pop()
    {
        PoolingObject obj = null;

        if(Pool.Count > 0)
        {
            obj = Pool.Dequeue();
        }
        else
        {
            Debug.Log("풀에 객체가 없어 새로운 객체를 생성합니다.");
            obj = CreateObject();
            OnPush?.Invoke(obj);
        }

        OnPop?.Invoke(obj);
        return obj;
    }

    public void Push(PoolingObject obj)
    {
        if(obj == null)
            return;

        OnPush?.Invoke(obj);
        Pool.Enqueue(obj);
    }
}
