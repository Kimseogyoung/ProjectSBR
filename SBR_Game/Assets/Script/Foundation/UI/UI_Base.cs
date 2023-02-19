using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

abstract public class UI_Base : MonoBehaviour
{
	protected string _exitButton = "ExitButton";
	private Dictionary<string, Object> _objects = new Dictionary<string, Object>();
    private Dictionary<string, Object[]> _objectLists = new Dictionary<string, Object[]>();

    //Bind UI ������Ʈ �̸����� ã�� ���ε����ֱ�
    protected T Bind<T>(string name) where T : UnityEngine.Object
	{
		Object obj = null;
        if (typeof(T) == typeof(GameObject))
            obj = Util.GameObj.FindChild(gameObject, name, true);
        else
            obj = Util.GameObj.FindChild<T>(gameObject, name, true);

		if (obj == null)
		{
            GameLogger.Error($"Failed to bind({name}) to {nameof(gameObject.name)}");
			return null;
        }

		_objects.Add(name, obj);
		return Get<T>(name);

    }

	protected List<T> BindMany<T>(string name) where T : UnityEngine.Object
	{
		Object[] objs;
        if (typeof(T) == typeof(GameObject))
            objs = Util.GameObj.FindChildAll(gameObject, name);
        else
            objs = Util.GameObj.FindChildAll<T>(gameObject, name);

        if (objs == null)
        {
            GameLogger.Error($"Failed to BindMany({name}) to {nameof(gameObject.name)}");
            return null;
        }

        _objectLists.Add(name, objs);
		return GetMany<T>(name);

    }

	//Get UI ������Ʈ ��������
	protected T Get<T>(string name) where T : UnityEngine.Object
	{
		UnityEngine.Object obj = null;
		if (_objects.TryGetValue(name, out obj) == false)
			return null;

		return obj as T;
	}

    protected List<T> GetMany<T>(string name) where T : UnityEngine.Object
    {
        Object[] objs;
        if (_objectLists.TryGetValue(name, out objs) == false)
            return null;

		List<T> list = new List<T>();
		for(int i=0; i<objs.Length; i++)
			list.Add(objs[i] as T);

        return list;
    }

    public void AddEvent(Button button, UnityAction unityAction)
	{
		button.onClick.AddListener(unityAction);
	}

	/*
	//BindEvent UI ������Ʈ�� �̺�Ʈ ����ϱ�
	public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
	{
		UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

		switch (type)
		{
			case Define.UIEvent.Click:
				evt.OnClickHandler -= action; // Ȥ�ó� �̹� ������� ����
				evt.OnClickHandler += action;
				break;
			case Define.UIEvent.Drag:
				evt.OnDragHandler -= action; // Ȥ�ó� �̹� ������� ����
				evt.OnDragHandler += action;
				break;
		}
	}
	*/

    public abstract void Init();
}