using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

{
	protected Dictionary<string, UnityEngine.Object> _objects = new Dictionary<string, UnityEngine.Object>();
	protected Dictionary<string, UnityEngine.Object[]> _listObjects = new Dictionary<string, UnityEngine.Object[]>();
	
	//Bind UI ������Ʈ �̸����� ã�� ���ε����ֱ�
	protected void Bind<T>(string name) where T : UnityEngine.Object
	{
		UnityEngine.Object obj = null;

		obj = Util.GameObj.FindChild<T>(gameObject, name, true);
		if(obj == null)
			GameLogger.Error($"Failed to bind({name})");

		_objects.Add(name, obj);
	}

	protected void BindMany<T>(string name, GameObject parentObject) where T : UnityEngine.Object
	{
		T[] objects = Util.GameObj.FindChildren<T>(parentObject);
		if (objects == null)
			GameLogger.Error($"Failed to bind({name})");

		_listObjects.Add(name, objects);
	}

	//Get UI ������Ʈ ��������
	protected T Get<T>(string name) where T : UnityEngine.Object
	{
		UnityEngine.Object obj = null;
		if (_objects.TryGetValue(name, out obj) == false)
			return null;

		return obj as T;
	}

	protected T[] GetMany<T>(string name) where T : UnityEngine.Object
	{
		UnityEngine.Object[] objects = null;
		if (_listObjects.TryGetValue(name, out objects) == false)
			return null;

		T[] ts = new T[objects.Length];
		for (int i = 0; i < objects.Length; i++)
			ts[i] = objects[i] as T;

		return ts;
	}

	//BindEvent UI ������Ʈ�� �̺�Ʈ ����ϱ�
	public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
	{
		UI_EventHandler evt = Util.GameObj.GetOrAddComponent<UI_EventHandler>(go);

		switch (type)
		{
			case Define.UIEvent.Click:
				evt.OnClickHandler -= action; 
				evt.OnClickHandler += action;
				break;
			case Define.UIEvent.Drag:
				evt.OnDragHandler -= action; 
				evt.OnDragHandler += action;
				break;
		}
	}

    public abstract void Init();
}
