
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

    int _order = 10; // 현재까지 최근에 사용한 오더

    Stack<UIPopup> _popupStack = new Stack<UIPopup>(); // 오브젝트 말고 컴포넌트를 담음. 팝업 캔버스 UI 들을 담는다.
    UIScene _sceneUI = null; // 현재의 고정 캔버스 UI
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
        canvas.overrideSorting = true; // 캔버스 안에 캔버스 중첩 경우 (부모 캔버스가 어떤 값을 가지던 나는 내 오더값을 가지려 할때)

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else // soring 요청 X 라는 소리는 팝업이 아닌 일반 고정 UI
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
        if (string.IsNullOrEmpty(name)) // 이름을 안받았다면 T로 ㄱㄱ
            name = typeof(T).Name;

        GameObject go = Util.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GameObj.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(Root.transform);

        return popup;
    }

    public void ClosePopupUI(UIPopup popup) // 안전 차원
    {
        if (_popupStack.Count == 0) // 비어있는 스택이라면 삭제 불가
            return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!"); // 스택의 가장 위에있는 Peek() 것만 삭제할 수 잇기 때문에 popup이 Peek()가 아니면 삭제 못함
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
        _order--; // order 줄이기
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }
}