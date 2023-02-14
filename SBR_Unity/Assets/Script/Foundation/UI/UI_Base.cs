using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

abstract public class UI_Base : MonoBehaviour
{
	protected string _exitButton = "ExitButton";
	private Dictionary<string, Object> _objects = new Dictionary<string, Object>();

	//Bind UI 오브젝트 이름으로 찾아 바인딩해주기
	protected void Bind<T>(string name) where T : UnityEngine.Object
	{
		Object obj = null;
        if (typeof(T) == typeof(GameObject))
            obj = Util.GameObj.FindChild(gameObject, name, true);
        else
            obj = Util.GameObj.FindChild<T>(gameObject, name, true);

		if (obj == null)
		{
            GameLogger.Error($"Failed to bind({name}) to {nameof(gameObject.name)}");
			return;
        }

		_objects.Add(name, obj);

    }

	//Get UI 오브젝트 가져오기
	protected T Get<T>(string name) where T : UnityEngine.Object
	{
		UnityEngine.Object obj = null;
		if (_objects.TryGetValue(name, out obj) == false)
			return null;

		return obj as T;
	}

	public void AddEvent(Button button, UnityAction unityAction)
	{
		button.onClick.AddListener(unityAction);
	}

	/*
	//BindEvent UI 오브젝트에 이벤트 등록하기
	public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
	{
		UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

		switch (type)
		{
			case Define.UIEvent.Click:
				evt.OnClickHandler -= action; // 혹시나 이미 있을까봐 빼줌
				evt.OnClickHandler += action;
				break;
			case Define.UIEvent.Drag:
				evt.OnDragHandler -= action; // 혹시나 이미 있을까봐 빼줌
				evt.OnDragHandler += action;
				break;
		}
	}
	*/

    public abstract void Init();
}
