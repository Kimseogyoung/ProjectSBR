using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup : UI_Base
{
    //�˾� UI �� ����, �˾� UI ĵ�������� �������� �κе�.
    protected override void InitImp()
    {
        APP.UI.SetCanvas(gameObject, true);
        if(Bind<Button>(_exitButton) != null)
            Get<Button>(_exitButton).onClick.AddListener(() => { ClosePopupUI(); });
    }

    public virtual void ClosePopupUI()  // �˾��̴ϱ� ���� ĵ����(Scene)�� �ٸ��� �ݴ°� �ʿ�
    {
        APP.UI.ClosePopupUI(this);
    }
}
