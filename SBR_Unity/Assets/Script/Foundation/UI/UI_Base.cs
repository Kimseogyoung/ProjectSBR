using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class UI_Base : MonoBehaviour
{
	//Bind UI ������Ʈ �̸����� ã�� ���ε����ֱ�
	protected void Bind<T>(Type type) where T : UnityEngine.Object
	{
		string[] names = Enum.GetNames(type);
		UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
		_objects.Add(typeof(T), objects); // Dictionary �� �߰�

		// T �� ���ϴ� ������Ʈ���� Dictionary�� Value�� objects �迭�� ���ҵ鿡 �ϳ��ϳ� �߰�
		for (int i = 0; i < names.Length; i++)
		{
			if (typeof(T) == typeof(GameObject))
				objects[i] = Util.FindChild(gameObject, names[i], true);
			else
				objects[i] = Util.FindChild<T>(gameObject, names[i], true);

			if (objects[i] == null)
				Debug.Log($"Failed to bind({names[i]})");
		}
	}
	//Get UI ������Ʈ ��������
	protected T Get<T>(int idx) where T : UnityEngine.Object
	{
		UnityEngine.Object[] objects = null;
		if (_objects.TryGetValue(typeof(T), out objects) == false)
			return null;

		return objects[idx] as T;
	}

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

    public abstract void Init();
}
