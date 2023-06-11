
using System;
using UnityEngine;
using System.Collections.Generic;

public class UIManager : IManager, IManagerUpdatable
{
    public void Init()
    {
    }

    public void StartManager()
    {
    }

    public void FinishManager()
    {
     
    }

    public void UpdateManager()
    {
        
    }

    public void UpdatePausedManager()
    {
       
    }

    public void Pause(bool IsPause)
    {
        
    }

    int _order = 10; // ������� �ֱٿ� ����� ����

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>(); // ������Ʈ ���� ������Ʈ�� ����. �˾� ĵ���� UI ���� ��´�.
    UI_Scene _sceneUI = null; // ������ ���� ĵ���� UI
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GameObj.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true; // ĵ���� �ȿ� ĵ���� ��ø ��� (�θ� ĵ������ � ���� ������ ���� �� �������� ������ �Ҷ�)

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else // soring ��û X ��� �Ҹ��� �˾��� �ƴ� �Ϲ� ���� UI
        {
            canvas.sortingOrder = 0;
        }
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        T sceneUI;
        GameObject go;
        if (!Util.GameObj.TryFind<T>(out sceneUI, name))
        {
            GameLogger.Info($"Create {name} scene");
            go = Util.Resource.Instantiate($"UI/Scene/{name}");
            sceneUI = Util.GameObj.GetOrAddComponent<T>(go);

        }
        else
        {
            go = sceneUI.gameObject;
        }

        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);
        _sceneUI.Init();

        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name)) // �̸��� �ȹ޾Ҵٸ� T�� ����
            name = typeof(T).Name;

        T popup = null;
        GameObject go = null;
        if (!Util.GameObj.TryFind<T>(out popup, name))
        {
            go = Util.Resource.Instantiate($"UI/Popup/{name}");
            popup = Util.GameObj.GetOrAddComponent<T>(go);
        }
        else
            go = popup.gameObject;

        _popupStack.Push(popup);

        go.transform.SetParent(Root.transform);
        popup.Init();

        return popup;
    }

    public void ClosePopupUI(UI_Popup popup) // ���� ����
    {
        if (_popupStack.Count == 0) // ����ִ� �����̶�� ���� �Ұ�
            return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!"); // ������ ���� �����ִ� Peek() �͸� ������ �� �ձ� ������ popup�� Peek()�� �ƴϸ� ���� ����
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        Util.GameObj.Destroy(popup.gameObject);
        popup = null;
        _order--; // order ���̱�
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

}