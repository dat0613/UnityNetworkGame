using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class CrashTest : MonoBehaviour
{
    public Gun prefab;
    Gun gun = null;
    Queue<int> messageQueue = new Queue<int>();
    Queue<int> tempQueue = new Queue<int>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // StartCoroutine("ReadMessage");
    }

    // IEnumerator ReadMessage()
    // {

    // }

    void Update()
    {
        while(tempQueue.Count > 0)
        {
            messageQueue.Enqueue(tempQueue.Dequeue());
        }

        while (messageQueue.Count > 0)
        {
            var message = messageQueue.Dequeue();

            if (message == 0)
            {
                // while(messageQueue.Count > 0)
                // {
                //     Debug.Log("다음 프레임으로 스킵");
                //     tempQueue.Enqueue(messageQueue.Dequeue());
                // }
                Debug.Log("씬 변경!");
                SceneManager.LoadScene("GameScene");
                Debug.Log("씬 변경 완료");
            }
            else if (message == 2)
            {
                Debug.Log("시작");
            }
            else if (message == 3)
            {
                Debug.Log("끗");
            }
            else if (message == 4)
            {
                CreateGun();
            }
            else
            {
                Debug.Log("대충 메세지");
            }
        }
   
        if(Input.GetKeyDown(KeyCode.A))
        {
            messageQueue.Enqueue(2);
            messageQueue.Enqueue(1);
            messageQueue.Enqueue(1);
            messageQueue.Enqueue(1);
            messageQueue.Enqueue(4);
            messageQueue.Enqueue(4);
            messageQueue.Enqueue(1);
            messageQueue.Enqueue(1);
            messageQueue.Enqueue(1);
            messageQueue.Enqueue(1);
            messageQueue.Enqueue(1);
            messageQueue.Enqueue(1);
            messageQueue.Enqueue(1);
            messageQueue.Enqueue(1);
            messageQueue.Enqueue(1);
            messageQueue.Enqueue(1);

            // messageQueue.Enqueue(0);

            messageQueue.Enqueue(1);
            messageQueue.Enqueue(1);
            messageQueue.Enqueue(1);
            messageQueue.Enqueue(4);
            messageQueue.Enqueue(4);
            messageQueue.Enqueue(4);
            messageQueue.Enqueue(1);
            messageQueue.Enqueue(1);
            messageQueue.Enqueue(1);
            messageQueue.Enqueue(3);
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            if(gun == null)
            {
                Debug.Log("총이 업다");
            }
            else
            {
                gun.Shot(null);
            }
        }
    }

    void CreateGun()
    {
                Debug.Log("생성");
                gun = Instantiate(prefab);
                gun.gameObject.name = "Wa Sanz";
    }

    void MessageHandler(int message)
    {

    }
}
