using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class ObjectPool<T> where T : Component, new() 
{
    private readonly Queue<GameObject> _queue = new Queue<GameObject>();
    private Transform _parentTransform;

    public ObjectPool(int maxCnt = 5, Transform parentTransform = null)
    {
        if (parentTransform == null)
        {
            GameObject parentObject = new GameObject();
            parentObject.name = typeof(T).Name + " Pool";
            _parentTransform = parentObject.transform;
        }
        else
        {
            _parentTransform = parentTransform;
        }

        for (int i = 0; i < maxCnt; i++)
        {
            CreateObject();
        }
    }
    
    public void Enqueue(GameObject gameObject)
    {
        gameObject.SetActive(false);
        _queue.Enqueue(gameObject);
    }

    public (GameObject, T) Dequeue()
    {
        if(_queue.Count <= 0 )
        {
            CreateObject();
        }

        GameObject gameObject = _queue.Dequeue();
        gameObject.SetActive(true);

        return (gameObject, gameObject.GetComponent<T>());
    }

    private void CreateObject()
    {
        GameObject gameObject = new GameObject();
        T component = gameObject.AddGetComponent<T>();

        gameObject.SetActive(false);
        gameObject.transform.SetParent(_parentTransform, true);

        _queue.Enqueue(gameObject);
    }
}

