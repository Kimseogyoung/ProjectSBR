using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class ObjectPool<T> where T : Component
{
    private readonly Queue<GameObject> _queue = new Queue<GameObject>();
    private Transform _parentTransform;
    private GameObject _defaultGameObject;

    public ObjectPool(int maxCnt = 5, Transform parentTransform = null, GameObject defaultGameObject = null)
    {
        _defaultGameObject = defaultGameObject;

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

    public void Destroy()
    {
        while(_queue.Count > 0)
        {
            var queueObj = _queue.Dequeue();
            GameObject.Destroy(queueObj);
        }

        GameObject.Destroy(_defaultGameObject);

        _queue.Clear();
        _parentTransform = null;
        _defaultGameObject = null;
    }

    public void Enqueue(GameObject gameObject)
    {
        gameObject.SetActive(false);
        _queue.Enqueue(gameObject);
    }

    public (GameObject gameObject, T component) Dequeue()
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
        GameObject newGameObject;
        if (_defaultGameObject != null)
        {
            newGameObject = GameObject.Instantiate(_defaultGameObject);
        }
        else
        {
            newGameObject = new GameObject();
        }

        T component = newGameObject.AddGetComponent<T>();

        newGameObject.SetActive(false);
        newGameObject.transform.SetParent(_parentTransform, true);

        _queue.Enqueue(newGameObject);
    }
}

