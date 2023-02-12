
using System;
using UnityEngine;
using System.Collections.Generic;

public class UIManager : IManager, IManagerUpdatable
{
    public void Init()
    {
    }

    public void PrepareManager()
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

    int _order = 10; // ������� �ֱٿ� ����� ����

    Stack<UIPopup> _popupStack = new Stack<UIPopup>(); // ������Ʈ ���� ������Ʈ�� ����. �˾� ĵ���� UI ���� ��´�.
    UIScene _sceneUI = null; // ������ ���� ĵ���� UI
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

    public T ShowSceneUI<T>(string name = null) where T : UIScene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

       // GameObject go = Util.Resource.Instantiate($"UI/Scene/{name}");
        GameObject goj =Resources.Load<GameObject>($"UI/Scene/{name}");
        GameObject go = GameObject.Instantiate(goj);
        T sceneUI = Util.GameObj.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null) where T : UIPopup
    {
        if (string.IsNullOrEmpty(name)) // �̸��� �ȹ޾Ҵٸ� T�� ����
            name = typeof(T).Name;

        GameObject go = Util.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GameObj.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(Root.transform);

        return popup;
    }

    public void ClosePopupUI(UIPopup popup) // ���� ����
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

        UIPopup popup = _popupStack.Pop();
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