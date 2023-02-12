using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    //�˾� UI �� ����, �˾� UI ĵ�������� �������� �κе�.
    public override void Init()
    {
        APP.UI.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopupUI()  // �˾��̴ϱ� ���� ĵ����(Scene)�� �ٸ��� �ݴ°� �ʿ�
    {
        APP.UI.ClosePopupUI(this);
    }
}
