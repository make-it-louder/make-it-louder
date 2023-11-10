using System.Collections.Generic;
using UnityEngine;
using System;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> _executionQueue = new Queue<Action>();

    public static UnityMainThreadDispatcher Instance { get; private set; }

    private void Awake()
    {
        // 싱글턴 패턴을 사용하여 인스턴스 관리
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        // 큐에서 작업을 실행
        while (_executionQueue.Count > 0)
        {
            _executionQueue.Dequeue().Invoke();
        }
    }

    public void ExecuteInUpdate(Action action)
    {
        // 큐에 작업을 추가
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(action);
        }
    }
}
